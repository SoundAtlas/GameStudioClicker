using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        // Core production state
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1000;
        public long LinesPerSecond { get; private set; } = 0;

        public IReadOnlyList<ActiveUpgrade> ActiveUpgrades { get; }
        public IReadOnlyList<WorkerUpgrade> WorkerUpgrades { get; }

        // Construction
        public GameState()
        {
            ActiveUpgrade mousePad = new ActiveUpgrade(
                id: "mouse_pad",
                displayName: "Mouse Pad",
                description: "Doubles lines of code per click",
                cost: 100,
                clickMultiplier: 2);

            ActiveUpgrade gamingMouse = new ActiveUpgrade(
                id: "gaming_mouse",
                displayName: "Gaming Mouse",
                description: "Doubles lines of code per click",
                cost: 400,
                clickMultiplier: 2,
                prerequisite: mousePad);

            ActiveUpgrade mechanicalKeyboard = new ActiveUpgrade(
                id: "mechanical_keyboard",
                displayName: "Mechanical Keyboard",
                description: "Doubles lines of code per click",
                cost: 700,
                clickMultiplier: 2,
                prerequisite: gamingMouse);

            ActiveUpgrade headset = new ActiveUpgrade(
                id: "headset",
                displayName: "Headset",
                description: "Doubles lines of code per click",
                cost: 800,
                clickMultiplier: 2,
                prerequisite: mechanicalKeyboard);

            ActiveUpgrade webcam = new ActiveUpgrade(
                id: "webcam",
                displayName: "Webcam",
                description: "Doubles lines of code per click",
                cost: 900,
                clickMultiplier: 2,
                prerequisite: headset);

            ActiveUpgrade externalSSD = new ActiveUpgrade(
                id: "external_ssd",
                displayName: "External SSD",
                description: "Doubles lines of code per click",
                cost: 1000,
                clickMultiplier: 2,
                prerequisite: webcam);

            ActiveUpgrade secondMonitor = new ActiveUpgrade(
                id: "second_monitor",
                displayName: "Second Monitor",
                description: "Doubles lines of code per click",
                cost: 1500,
                clickMultiplier: 2,
                prerequisite: externalSSD);

            ActiveUpgrade ultrawideMonitor = new ActiveUpgrade(
                id: "ultrawide_monitor",
                displayName: "Ultrawide Monitor",
                description: "Triples lines of code per click",
                cost: 3000,
                clickMultiplier: 3,
                prerequisite: secondMonitor);

            //9   Developer Laptop    
            //10  Developer PC    
            //11  High - End Workstation    
            //12  Home Server 
            //13  AI Workstation  
            //14  Rack Server 
            //15  Server Rack 
            //16  Server Room 
            //17  Small Data Center   
            //18  Enterprise Data Center  
            //19  Hyperscale Data Center  
            //20  Supercomputer   


            ActiveUpgrades = new List<ActiveUpgrade>
            {
                mousePad,
                gamingMouse,
                mechanicalKeyboard,
                headset,
                webcam,
                externalSSD,
                secondMonitor,
                ultrawideMonitor
            };

            WorkerUpgrade intern = new WorkerUpgrade(
                id: "intern",
                displayName: "Intern",
                description: "Produces 2 lines of code per second",
                baseCost: 50,
                baseLinesPerSecond: 2);

            WorkerUpgrade juniorDeveloper = new WorkerUpgrade(
                id: "junior_developer",
                displayName: "Junior Developer",
                description: "Produces 20 lines of code per second",
                baseCost: 2000,
                baseLinesPerSecond: 20,
                prerequisite: intern,
                requiredPrerequisiteCount: 5);

            // Additional worker upgrades can be added here in the future, following the same pattern.

            WorkerUpgrades = new List<WorkerUpgrade>
            {
                intern,
                juniorDeveloper
            };
        }

        // Persistence

        // Creates an independent snapshot containing only values that need to be saved.
        public GameSaveData CreateSaveData()
        {
            var saveData = new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
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

            return saveData;
        }

        // Restores saved values and rebuilds production rates and future upgrade costs.
        public void RestoreFromSaveData(GameSaveData saveData)
        {
            // Reject null save data before reading any values from it.
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData), "Save data cannot be null.");
            }

            // Clamp persisted values in case the save file was edited or corrupted.
            LinesOfCode = Math.Max(0L, saveData.LinesOfCode);


            // Restore active upgrade counts and recalculate their costs.
            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                bool isPurchased = saveData.PurchasedActiveUpgradeIds.Contains(upgrade.Id);
                upgrade.RestorePurchaseState(isPurchased);
            }

            // Apply click multipliers from purchased active upgrades.
            LinesPerClick = 1;
            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    LinesPerClick *= upgrade.ClickMultiplier;
                }
            }

            // Restore worker upgrade counts and recalculate their costs.
            foreach (WorkerUpgrade upgrade in WorkerUpgrades)
            {
                // If the save data does not contain a count for this upgrade, default to 0.
                saveData.WorkerUpgradeCounts.TryGetValue(upgrade.Id, out int savedCount);

                // Restore the worker count and recalculate the current cost based on the saved count.
                upgrade.RestoreWorkerCount(savedCount);
            }

            RecalculateLinesPerSecond();
        }

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
                return true;
            }
            return false;
        }

        private void RecalculateLinesPerSecond()
        {
            long calculatedLinesPerSecond = 0;
            foreach (var upgrade in WorkerUpgrades)
            {
                calculatedLinesPerSecond += upgrade.TotalLinesPerSecond;
            }
            LinesPerSecond = calculatedLinesPerSecond;
        }

        // Code generation
        public void WriteCode()
        {
            LinesOfCode += LinesPerClick;
        }

        public void GeneratePassiveLines()
        {
            LinesOfCode += LinesPerSecond;
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

            LinesOfCode += offlineLines;

            return offlineLines;
        }
    }
}
