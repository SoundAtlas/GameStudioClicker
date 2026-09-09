using GameStudioClicker.Core.Content;
using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        // Core production state
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 999;
        public long LinesPerSecond { get; private set; } = 0;

        public IReadOnlyList<ActiveUpgrade> ActiveUpgrades { get; }
        public IReadOnlyList<WorkerUpgrade> WorkerUpgrades { get; }
        public IReadOnlyList<Achievement> Achievements { get; }

        // Lifetime stats
        public long LifetimeLinesOfCode { get; private set; }
        public long LifetimeManualClicks { get; private set; }
        public long LifetimeEmployeesHired { get; private set; }
        public long LifetimeActiveUpgradesPurchased { get; private set; }
        public long LinesGeneratedManually { get; private set; }
        public long LinesGeneratedByWorkers { get; private set; }
        public long LinesGeneratedWhileOnline { get; private set; }
        public long LinesGeneratedWhileOffline { get; private set; }

        // Construction
        public GameState()
        {
            ActiveUpgrades = GameContentFactory.CreateActiveUpgrades();
            WorkerUpgrades = GameContentFactory.CreateWorkerUpgrades();
            Achievements = GameContentFactory.CreateAchievements();
        }

        // Resource generation
        public void WriteCode()
        {
            AddLinesOfCode(LinesPerClick);
            LinesGeneratedManually += LinesPerClick;
            LinesGeneratedWhileOnline += LinesPerClick;
            LifetimeManualClicks++;

            CheckForNewAchievements();
        }

        public void GeneratePassiveLines()
        {
            AddLinesOfCode(LinesPerSecond);
            LinesGeneratedByWorkers += LinesPerSecond;
            LinesGeneratedWhileOnline += LinesPerSecond;

            CheckForNewAchievements();
        }

        public long ApplyOfflineProgress(TimeSpan elapsed)
        {
            if (elapsed.TotalSeconds <= 0)
            {
                return 0;
            }

            // Limit offline earnings to 24 hours of passive production.
            var maxElapsed = TimeSpan.FromHours(24);
            if (elapsed > maxElapsed)
            {
                elapsed = maxElapsed;
            }

            // Fractional seconds do not produce partial lines of code.
            long wholeSeconds = (long)elapsed.TotalSeconds;

            long offlineLines = wholeSeconds * LinesPerSecond;

            AddLinesOfCode(offlineLines);
            LinesGeneratedByWorkers += offlineLines;
            LinesGeneratedWhileOffline += offlineLines;

            CheckForNewAchievements();
            return offlineLines;
        }

        private void AddLinesOfCode(long amount)
        {
            LinesOfCode += amount;
            LifetimeLinesOfCode += amount;
        }

        // Purchase rules and actions
        public bool CanAffordUpgrade(long cost)
        {
            return LinesOfCode >= cost;
        }

        public bool CanPurchaseActiveUpgrade(ActiveUpgrade activeUpgrade)
        {
            if (ActiveUpgrades.Contains(activeUpgrade) &&
                activeUpgrade.IsAvailable &&
                CanAffordUpgrade(activeUpgrade.Cost))
            {
                if (activeUpgrade.TargetWorkerId != null)
                {
                    foreach (WorkerUpgrade workerUpgrade in WorkerUpgrades)
                    {
                        if (workerUpgrade.Id == activeUpgrade.TargetWorkerId)
                        {
                            return workerUpgrade.WorkerCount > 0;
                        }
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        public bool TryPurchaseActiveUpgrade(ActiveUpgrade activeUpgrade)
        {
            if (CanPurchaseActiveUpgrade(activeUpgrade))
            {
                LinesOfCode -= activeUpgrade.Cost;
                LinesPerClick *= activeUpgrade.ClickMultiplier;
                activeUpgrade.MarkAsPurchased();
                RecalculateLinesPerSecond();
                LifetimeActiveUpgradesPurchased++;

                CheckForNewAchievements();

                return true;
            }

            return false;
        }

        public bool CanPurchaseWorkerUpgrade(WorkerUpgrade workerUpgrade)
        {
            if (WorkerUpgrades.Contains(workerUpgrade) &&
                workerUpgrade.IsUnlocked &&
                CanAffordUpgrade(workerUpgrade.CurrentCost))
            {
                return true;
            }

            return false;
        }

        public bool TryPurchaseWorkerUpgrade(WorkerUpgrade workerUpgrade)
        {
            if (CanPurchaseWorkerUpgrade(workerUpgrade))
            {
                LinesOfCode -= workerUpgrade.CurrentCost;
                workerUpgrade.AddWorker();
                RecalculateLinesPerSecond();
                LifetimeEmployeesHired++;

                CheckForNewAchievements();

                return true;
            }

            return false;
        }

        // Worker production queries
        public long GetWorkerLinesPerSecond(WorkerUpgrade worker)
        {
            return worker.WorkerCount * GetWorkerLinesPerSecondPerEmployee(worker);
        }

        public long GetWorkerLinesPerSecondPerEmployee(WorkerUpgrade worker)
        {
            long linesPerSecond = worker.BaseLinesPerSecond;

            foreach (var activeUpgrade in ActiveUpgrades)
            {
                if (activeUpgrade.IsPurchased &&
                    (activeUpgrade.TargetWorkerId == worker.Id || activeUpgrade.TargetAllWorkers))
                {
                    linesPerSecond *= activeUpgrade.WorkerProductionMultiplier;
                }
            }

            return linesPerSecond;
        }

        // Achievements
        public IReadOnlyList<Achievement> CheckForNewAchievements()
        {
            var newlyEarnedAchievements = new List<Achievement>();
            foreach (var achievement in Achievements)
            {
                if (achievement.IsEarned)
                {
                    continue;
                }

                long progress =
                    GetAchievementProgress(achievement);
                if (progress >= achievement.RequirementValue)
                {
                    achievement.MarkAsEarned();
                    newlyEarnedAchievements.Add(achievement);
                }
            }

            return newlyEarnedAchievements;
        }

        public long GetAchievementProgress(Achievement achievement)
        {
            if (achievement is null)
            {
                throw new ArgumentNullException(nameof(achievement));
            }

            return achievement.RequirementType switch
            {
                AchievementRequirementType.ManualClicks => LifetimeManualClicks,
                AchievementRequirementType.LifetimeLinesOfCode => LifetimeLinesOfCode,
                AchievementRequirementType.EmployeesHired => LifetimeEmployeesHired,
                AchievementRequirementType.ActiveUpgradesPurchased => LifetimeActiveUpgradesPurchased,
                _ => throw new ArgumentOutOfRangeException(nameof(achievement.RequirementType)),
            };
        }

        // Persistence
        public GameSaveData CreateSaveData()
        {
            var saveData = new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
                LifetimeLinesOfCode = this.LifetimeLinesOfCode,
                LifetimeManualClicks = this.LifetimeManualClicks,
                LifetimeEmployeesHired = this.LifetimeEmployeesHired,
                LifetimeActiveUpgradesPurchased = this.LifetimeActiveUpgradesPurchased,
                LinesGeneratedByWorkers = this.LinesGeneratedByWorkers,
                LinesGeneratedManually = this.LinesGeneratedManually,
                LinesGeneratedWhileOnline = this.LinesGeneratedWhileOnline,
                LinesGeneratedWhileOffline = this.LinesGeneratedWhileOffline,
            };

            foreach (var upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    saveData.PurchasedActiveUpgradeIds.Add(upgrade.Id);
                }
            }

            foreach (var workerUpgrade in WorkerUpgrades)
            {
                saveData.WorkerUpgradeCounts[workerUpgrade.Id] = workerUpgrade.WorkerCount;
            }

            foreach (var achievement in Achievements)
            {
                if (achievement.IsEarned)
                {
                    saveData.EarnedAchievementIds.Add(achievement.Id);
                }
            }

            return saveData;
        }

        // Derived production rates and costs are rebuilt from persistent values.
        public void RestoreFromSaveData(GameSaveData saveData)
        {
            if (saveData is null)
            {
                throw new ArgumentNullException(nameof(saveData), "Save data cannot be null.");
            }

            List<string> purchasedActiveUpgradeIds =
                saveData.PurchasedActiveUpgradeIds ?? [];
            Dictionary<string, int> workerUpgradeCounts =
                saveData.WorkerUpgradeCounts ?? [];
            List<string> earnedAchievementIds =
                saveData.EarnedAchievementIds ?? [];

            LinesOfCode = Math.Max(0L, saveData.LinesOfCode);

            // Statistics
            LifetimeLinesOfCode = Math.Max(0L, saveData.LifetimeLinesOfCode);
            LifetimeManualClicks = Math.Max(0L, saveData.LifetimeManualClicks);
            LifetimeEmployeesHired = Math.Max(0L, saveData.LifetimeEmployeesHired);
            LifetimeActiveUpgradesPurchased = Math.Max(0L, saveData.LifetimeActiveUpgradesPurchased);
            LinesGeneratedManually = Math.Max(0L, saveData.LinesGeneratedManually);
            LinesGeneratedByWorkers = Math.Max(0L, saveData.LinesGeneratedByWorkers);
            LinesGeneratedWhileOnline = Math.Max(0L, saveData.LinesGeneratedWhileOnline);
            LinesGeneratedWhileOffline = Math.Max(0L, saveData.LinesGeneratedWhileOffline);

            foreach (var upgrade in ActiveUpgrades)
            {
                bool isPurchased = purchasedActiveUpgradeIds.Contains(upgrade.Id);
                upgrade.RestorePurchaseState(isPurchased);
            }

            LinesPerClick = 1;
            foreach (var upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    LinesPerClick *= upgrade.ClickMultiplier;
                }
            }

            foreach (var upgrade in WorkerUpgrades)
            {
                // Missing worker IDs represent a saved count of zero.
                workerUpgradeCounts.TryGetValue(upgrade.Id, out int savedCount);
                upgrade.RestoreWorkerCount(savedCount);
            }

            foreach (var achievement in Achievements)
            {
                bool isEarned = earnedAchievementIds.Contains(achievement.Id);
                achievement.RestoreEarnedState(isEarned);
            }

            RecalculateLinesPerSecond();

            CheckForNewAchievements();
        }

        private void RecalculateLinesPerSecond()
        {
            long calculatedLinesPerSecond = 0;

            foreach (var worker in WorkerUpgrades)
            {
                long linesPerSecond = GetWorkerLinesPerSecond(worker);
                calculatedLinesPerSecond += linesPerSecond;
            }

            LinesPerSecond = calculatedLinesPerSecond;
        }
    }
}
