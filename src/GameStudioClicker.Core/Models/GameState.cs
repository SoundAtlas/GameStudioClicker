using GameStudioClicker.Core.Persistence;

namespace GameStudioClicker.Core.Models
{
    public class GameState
    {
        // Core production state
        public long LinesOfCode { get; private set; }
        public long LinesPerClick { get; private set; } = 1;
        public long LinesPerSecond { get; private set; } = 0;

        public IReadOnlyList<ActiveUpgrade> ActiveUpgrades { get; }
        public IReadOnlyList<WorkerUpgrade> WorkerUpgrades { get; }

        // Construction
        public GameState()
        {
            ActiveUpgrade mousePad = new ActiveUpgrade(
                id: "mouse_pad",
                displayName: "Mouse Pad",
                description: "Doubles Lines of Code / Click",
                cost: 100,
                clickMultiplier: 2);

            ActiveUpgrade gamingMouse = new ActiveUpgrade(
                id: "gaming_mouse",
                displayName: "Gaming Mouse",
                description: "Doubles Lines of Code / Click",
                cost: 400,
                clickMultiplier: 2,
                prerequisite: mousePad);

            ActiveUpgrade mechanicalKeyboard = new ActiveUpgrade(
                id: "mechanical_keyboard",
                displayName: "Mechanical Keyboard",
                description: "Doubles Lines of Code / Click",
                cost: 700,
                clickMultiplier: 2,
                prerequisite: gamingMouse);

            ActiveUpgrade headset = new ActiveUpgrade(
                id: "headset",
                displayName: "Headset",
                description: "Doubles Lines of Code / Click",
                cost: 800,
                clickMultiplier: 2,
                prerequisite: mechanicalKeyboard);

            ActiveUpgrade webcam = new ActiveUpgrade(
                id: "webcam",
                displayName: "Webcam",
                description: "Doubles Lines of Code / Click",
                cost: 900,
                clickMultiplier: 2,
                prerequisite: headset);

            ActiveUpgrade internTrainingManual = new ActiveUpgrade(
                id: "intern_training_manual",
                displayName: "Intern Training Manual",
                description: "Doubles Intern Productivity",
                cost: 2000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "intern",
                prerequisite: webcam);

            ActiveUpgrade externalSSD = new ActiveUpgrade(
                id: "external_ssd",
                displayName: "External SSD",
                description: "Doubles Lines of Code / Click",
                cost: 1000,
                clickMultiplier: 2,
                prerequisite: webcam);

            ActiveUpgrade secondMonitor = new ActiveUpgrade(
                id: "second_monitor",
                displayName: "Second Monitor",
                description: "Doubles Lines of Code / Click",
                cost: 1500,
                clickMultiplier: 2,
                prerequisite: externalSSD);

            ActiveUpgrade ultrawideMonitor = new ActiveUpgrade(
                id: "ultrawide_monitor",
                displayName: "Ultrawide Monitor",
                description: "Triples Lines of Code / Click",
                cost: 3000,
                clickMultiplier: 3,
                prerequisite: secondMonitor);

            ActiveUpgrade developerLaptop = new ActiveUpgrade(
                id: "developer_laptop",
                displayName: "Developer Laptop",
                description: "Doubles Lines of Code / Click",
                cost: 6000,
                clickMultiplier: 2,
                prerequisite: ultrawideMonitor);

            ActiveUpgrade codeReviewChecklist = new ActiveUpgrade(
                id: "code_review_checklist",
                displayName: "Code Review Checklist",
                description: "Doubles Junior Developer Productivity",
                cost: 12000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "junior_developer",
                prerequisite: developerLaptop);

            ActiveUpgrade developerPc = new ActiveUpgrade(
                id: "developer_pc",
                displayName: "Developer PC",
                description: "Doubles Lines of Code / Click",
                cost: 25000,
                clickMultiplier: 2,
                prerequisite: developerLaptop);

            ActiveUpgrade highEndWorkstation = new ActiveUpgrade(
                id: "high_end_workstation",
                displayName: "High-End Workstation",
                description: "Doubles Lines of Code / Click",
                cost: 50000,
                clickMultiplier: 2,
                prerequisite: developerPc);

            ActiveUpgrade internMentorshipProgram = new ActiveUpgrade(
                id: "intern_mentorship_program",
                displayName: "Intern Mentorship Program",
                description: "Triples Intern Productivity",
                cost: 100000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "intern",
                prerequisite: highEndWorkstation);

            ActiveUpgrade homeServer = new ActiveUpgrade(
                id: "home_server",
                displayName: "Home Server",
                description: "Doubles Lines of Code / Click",
                cost: 150000,
                clickMultiplier: 2,
                prerequisite: highEndWorkstation);

            ActiveUpgrade architechtureWorkshop = new ActiveUpgrade(
                id: "architecture_workshop",
                displayName: "Architecture Workshop",
                description: "Doubles Senior Developer Productivity",
                cost: 200000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "senior_developer",
                prerequisite: homeServer);

            ActiveUpgrade aiWorkstation = new ActiveUpgrade(
                id: "ai_workstation",
                displayName: "AI Workstation",
                description: "4x Lines of Code / Click",
                cost: 250000,
                clickMultiplier: 4,
                prerequisite: homeServer);

            ActiveUpgrade rackServer = new ActiveUpgrade(
                id: "rack_server",
                displayName: "Rack Server",
                description: "Doubles Lines of Code / Click",
                cost: 500000,
                clickMultiplier: 2,
                prerequisite: aiWorkstation);

            ActiveUpgrade pairProgrammingSessions = new ActiveUpgrade(
                id: "pair_programming_sessions",
                displayName: "Pair Programming Sessions",
                description: "Triples Junior Developer Productivity",
                cost: 750000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "junior_developer",
                prerequisite: developerLaptop);

            ActiveUpgrade serverRoom = new ActiveUpgrade(
                id: "server_room",
                displayName: "Server Room",
                description: "Doubles Lines of Code / Click",
                cost: 1000000,
                clickMultiplier: 2,
                prerequisite: rackServer);

            ActiveUpgrade smallDataCenter = new ActiveUpgrade(
                id: "small_data_center",
                displayName: "Small Data Center",
                description: "Triples Lines of Code / Click",
                cost: 3500000,
                clickMultiplier: 3,
                prerequisite: serverRoom);

            ActiveUpgrade enterpriseDataCenter = new ActiveUpgrade(
                id: "enterprise_data_center",
                displayName: "Enterprise Data Center",
                description: "Doubles Lines of Code / Click",
                cost: 85000000,
                clickMultiplier: 2,
                prerequisite: smallDataCenter);

            ActiveUpgrade advancedToolingLicense = new ActiveUpgrade(
                id: "advanced_tooling_license",
                displayName: "Advanced Tooling License",
                description: "Triples Senior Developer Productivity",
                cost: 12000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "senior_developer",
                prerequisite: enterpriseDataCenter);

            ActiveUpgrade hyperscaleDataCenter = new ActiveUpgrade(
                id: "hyperscale_data_center",
                displayName: "Hyperscale Data Center",
                description: "Doubles Lines of Code / Click",
                cost: 125000000,
                clickMultiplier: 2,
                prerequisite: enterpriseDataCenter);

            ActiveUpgrade leadershipCoaching = new ActiveUpgrade(
                id: "leadership_coaching",
                displayName: "Leadership Coaching",
                description: "Doubles Lead Developer Productivity",
                cost: 175000000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "lead_developer",
                prerequisite: hyperscaleDataCenter);

            ActiveUpgrade superComputer = new ActiveUpgrade(
                id: "super_computer",
                displayName: "Super Computer",
                description: "5x Lines of Code / Click",
                cost: 250000000,
                clickMultiplier: 5,
                prerequisite: hyperscaleDataCenter);

            ActiveUpgrade technicalStrategySummit = new ActiveUpgrade(
                id: "technical_strategy_summit",
                displayName: "Technical Strategy Summit",
                description: "Triples Lead Developer Productivity",
                cost: 175000000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "lead_developer",
                prerequisite: superComputer);

            ActiveUpgrade automatedDevPipeline = new ActiveUpgrade(
                id: "automated_dev_pipeline",
                displayName: "Automated Dev Pipeline",
                description: "4x All Worker Productivity",
                cost: 350000000,
                clickMultiplier: 1,
                workerProductionMultiplier: 4,
                targetAllWorkers: true,
                prerequisite: technicalStrategySummit);

            // Additional hardware upgrades can extend this ordered progression.


            ActiveUpgrades = new List<ActiveUpgrade>
            {
                mousePad,
                gamingMouse,
                mechanicalKeyboard,
                headset,
                webcam,
                internTrainingManual,
                externalSSD,
                secondMonitor,
                ultrawideMonitor,
                developerLaptop,
                codeReviewChecklist,
                developerPc,
                highEndWorkstation,
                internMentorshipProgram,
                homeServer,
                architechtureWorkshop,
                aiWorkstation,
                rackServer,
                pairProgrammingSessions,
                serverRoom,
                smallDataCenter,
                enterpriseDataCenter,
                advancedToolingLicense,
                hyperscaleDataCenter,
                leadershipCoaching,
                superComputer,
                technicalStrategySummit,
                automatedDevPipeline
            };

            WorkerUpgrade intern = new WorkerUpgrade(
                id: "intern",
                displayName: "Intern",
                baseCost: 50,
                baseLinesPerSecond: 2);

            WorkerUpgrade juniorDeveloper = new WorkerUpgrade(
                id: "junior_developer",
                displayName: "Junior Developer",
                baseCost: 2000,
                baseLinesPerSecond: 20,
                prerequisite: intern,
                requiredPrerequisiteCount: 5);

            WorkerUpgrade seniorDeveloper = new WorkerUpgrade(
                id: "senior_developer",
                displayName: "Senior Developer",
                baseCost: 20000,
                baseLinesPerSecond: 2000,
                prerequisite: juniorDeveloper,
                requiredPrerequisiteCount: 5);

            WorkerUpgrade leadDeveloper = new WorkerUpgrade(
                id: "lead_developer",
                displayName: "Lead Developer",
                baseCost: 200000,
                baseLinesPerSecond: 20000,
                prerequisite: seniorDeveloper,
                requiredPrerequisiteCount: 1);

            WorkerUpgrades = new List<WorkerUpgrade>
            {
                intern,
                juniorDeveloper,
                seniorDeveloper,
                leadDeveloper,
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

            // Restore the one-time purchase state of every active upgrade.
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

            // Restore worker counts; each worker rebuilds its own current cost.
            foreach (WorkerUpgrade upgrade in WorkerUpgrades)
            {
                // Missing dictionary entries default to zero through TryGetValue.
                saveData.WorkerUpgradeCounts.TryGetValue(upgrade.Id, out int savedCount);
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
                RecalculateLinesPerSecond();
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
            foreach (WorkerUpgrade worker in WorkerUpgrades)
            {
                long linesPerSecond = GetWorkerLinesPerSecond(worker);

                calculatedLinesPerSecond += linesPerSecond;
            }

            LinesPerSecond = calculatedLinesPerSecond;
        }

        public long GetWorkerLinesPerSecond(WorkerUpgrade worker)
        {
            long linesPerSecond = worker.TotalLinesPerSecond;
            foreach (ActiveUpgrade activeUpgrade in ActiveUpgrades)
            {
                if (activeUpgrade.IsPurchased && (activeUpgrade.TargetWorkerId == worker.Id || activeUpgrade.TargetAllWorkers))
                {
                    linesPerSecond *= activeUpgrade.WorkerProductionMultiplier;
                }
            }

            return linesPerSecond;
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
