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

        // Lifetime stats
        public long LifetimeLinesOfCode { get; private set; }

        // Construction
        public GameState()
        {
            // Era 1: Bedroom Developer
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
                cost: 750,
                clickMultiplier: 2,
                prerequisite: gamingMouse);

            ActiveUpgrade noiseCancellingHeadset = new ActiveUpgrade(
                id: "noise_cancelling_headset",
                displayName: "Noise Cancelling Headset",
                description: "Doubles Lines of Code / Click",
                cost: 900,
                clickMultiplier: 2,
                prerequisite: mechanicalKeyboard);

            ActiveUpgrade onboardingHandbook = new ActiveUpgrade(
                id: "onboarding_handbook",
                displayName: "Onboarding Handbook",
                description: "Doubles Intern Productivity",
                cost: 1200,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "intern",
                prerequisite: noiseCancellingHeadset);

            ActiveUpgrade graphicsCard = new ActiveUpgrade(
                id: "graphics_card",
                displayName: "Graphics Card",
                description: "Doubles Lines of Code / Click",
                cost: 1500,
                clickMultiplier: 2,
                prerequisite: noiseCancellingHeadset);

            ActiveUpgrade secondMonitor = new ActiveUpgrade(
                id: "second_monitor",
                displayName: "Second Monitor",
                description: "Doubles Lines of Code / Click",
                cost: 2250,
                clickMultiplier: 2,
                prerequisite: graphicsCard);

            ActiveUpgrade ergonomicDeskSetup = new ActiveUpgrade(
                id: "ergonomic_desk_setup",
                displayName: "Ergonomic Desk Setup",
                description: "Triples Lines of Code / Click",
                cost: 3500,
                clickMultiplier: 3,
                prerequisite: secondMonitor);


            // Era 2: Tiny Indie Studio
            ActiveUpgrade developerLaptop = new ActiveUpgrade(
                id: "developer_laptop",
                displayName: "Developer Laptop",
                description: "Doubles Lines of Code / Click",
                cost: 6000,
                clickMultiplier: 2,
                prerequisite: ergonomicDeskSetup);

            ActiveUpgrade codeReviewChecklist = new ActiveUpgrade(
                id: "code_review_checklist",
                displayName: "Code Review Checklist",
                description: "Doubles Junior Developer Productivity",
                cost: 12000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "junior_developer",
                prerequisite: developerLaptop);

            ActiveUpgrade professionalIdeLicense = new ActiveUpgrade(
                id: "professional_ide_license",
                displayName: "Professional IDE License",
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
                prerequisite: professionalIdeLicense);

            ActiveUpgrade internMentorshipProgram = new ActiveUpgrade(
                id: "intern_mentorship_program",
                displayName: "Intern Mentorship Program",
                description: "Triples Intern Productivity",
                cost: 100000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "intern",
                prerequisite: highEndWorkstation);

            ActiveUpgrade buildServer = new ActiveUpgrade(
                id: "build_server",
                displayName: "Build Server",
                description: "Doubles Lines of Code / Click",
                cost: 150000,
                clickMultiplier: 2,
                prerequisite: highEndWorkstation);

            ActiveUpgrade automatedTestingSuite = new ActiveUpgrade(
                id: "automated_testing_suite",
                displayName: "Automated Testing Suite",
                description: "Triples Lines of Code / Click",
                cost: 200000,
                clickMultiplier: 3,
                prerequisite: buildServer);

            ActiveUpgrade architectureWorkshop = new ActiveUpgrade(
                id: "architecture_workshop",
                displayName: "Architecture Workshop",
                description: "Doubles Senior Developer Productivity",
                cost: 250000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "senior_developer",
                prerequisite: automatedTestingSuite);



            // Era 3: Established Game Studio  
            ActiveUpgrade automatedDevelopmentPipeline = new ActiveUpgrade(
                id: "automated_development_pipeline",
                displayName: "Automated Development Pipeline",
                description: "4x All Worker Productivity",
                cost: 350000,
                clickMultiplier: 1,
                workerProductionMultiplier: 4,
                targetAllWorkers: true,
                prerequisite: automatedTestingSuite);

            ActiveUpgrade captureStudio = new ActiveUpgrade(
                id: "capture_studio",
                displayName: "Motion-Capture Studio",
                description: "Doubles Lines of Code / Click",
                cost: 400000,
                clickMultiplier: 2,
                prerequisite: automatedDevelopmentPipeline);

            ActiveUpgrade pairProgrammingSessions = new ActiveUpgrade(
                id: "pair_programming_sessions",
                displayName: "Pair Programming Sessions",
                description: "Triples Junior Developer Productivity",
                cost: 750000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "junior_developer",
                prerequisite: captureStudio);

            ActiveUpgrade studioServerRack = new ActiveUpgrade(
                id: "studio_server_rack",
                displayName: "Studio Server Rack",
                description: "Doubles Lines of Code / Click",
                cost: 1000000,
                clickMultiplier: 2,
                prerequisite: captureStudio);

            ActiveUpgrade proprietaryGameEngine = new ActiveUpgrade(
                id: "proprietary_game_engine",
                displayName: "Proprietary Game Engine",
                description: "Doubles Lines of Code / Click",
                cost: 1500000,
                clickMultiplier: 2,
                prerequisite: studioServerRack);

            ActiveUpgrade technicalLeadershipTraining = new ActiveUpgrade(
                id: "technical_leadership_training",
                displayName: "Technical Leadership Training",
                description: "Doubles Lead Developer Productivity",
                cost: 2000000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "lead_developer",
                prerequisite: proprietaryGameEngine);

            ActiveUpgrade renderFarm = new ActiveUpgrade(
                id: "render_farm",
                displayName: "Render Farm",
                description: "Triples Lines of Code / Click",
                cost: 2500000,
                clickMultiplier: 3,
                prerequisite: proprietaryGameEngine);

            ActiveUpgrade globalCloudInfrastructure = new ActiveUpgrade(
                id: "global_cloud_infrastructure",
                displayName: "Global Cloud Infrastructure",
                description: "Doubles Lines of Code / Click",
                cost: 3000000,
                clickMultiplier: 3,
                prerequisite: renderFarm);


            // Era 4 Cutting-Edge Megastudio

            ActiveUpgrade aiWorkstations = new ActiveUpgrade(
                id: "ai_workstations",
                displayName: "AI Workstations",
                description: "4x Lines of Code / Click",
                cost: 350000,
                clickMultiplier: 4,
                prerequisite: automatedTestingSuite);

            //Proprietary Developer Toolkit — Senior Developer targeted
            //Neural Motion - Capture System
            //Autonomous QA Swarm
            //Self - Organizing Dev Teams — Lead Developer targeted
            //Quantum Build Server
            //Predictive Game Engine
            //Adaptive Learning Program — Intern targeted





            // Additional hardware upgrades can extend this ordered progression.
            ActiveUpgrades = new List<ActiveUpgrade>
            {
                mousePad,
                gamingMouse,
                mechanicalKeyboard,
                noiseCancellingHeadset,
                onboardingHandbook,
                graphicsCard,
                secondMonitor,
                ergonomicDeskSetup,

                developerLaptop,
                codeReviewChecklist,
                professionalIdeLicense,
                highEndWorkstation,
                internMentorshipProgram,
                buildServer,
                automatedTestingSuite,
                architectureWorkshop,

                automatedDevelopmentPipeline,
                codeReviewChecklist,
                professionalIdeLicense,
                highEndWorkstation,
                internMentorshipProgram,
                buildServer,
                automatedTestingSuite,
                architectureWorkshop,

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

        // Resource generation
        public void WriteCode()
        {
            AddLinesOfCode(LinesPerClick);
        }

        public void GeneratePassiveLines()
        {
            AddLinesOfCode(LinesPerSecond);
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

        // Worker production queries
        public long GetWorkerLinesPerSecond(WorkerUpgrade worker)
        {
            return worker.WorkerCount * GetWorkerLinesPerSecondPerEmployee(worker);
        }

        public long GetWorkerLinesPerSecondPerEmployee(WorkerUpgrade worker)
        {
            long linesPerSecond = worker.BaseLinesPerSecond;

            foreach (ActiveUpgrade activeUpgrade in ActiveUpgrades)
            {
                if (activeUpgrade.IsPurchased &&
                    (activeUpgrade.TargetWorkerId == worker.Id || activeUpgrade.TargetAllWorkers))
                {
                    linesPerSecond *= activeUpgrade.WorkerProductionMultiplier;
                }
            }

            return linesPerSecond;
        }

        // Persistence
        public GameSaveData CreateSaveData()
        {
            var saveData = new GameSaveData
            {
                LinesOfCode = this.LinesOfCode,
                LifetimeLinesOfCode = this.LifetimeLinesOfCode
            };

            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    saveData.PurchasedActiveUpgradeIds.Add(upgrade.Id);
                }
            }

            foreach (WorkerUpgrade workerUpgrade in WorkerUpgrades)
            {
                saveData.WorkerUpgradeCounts[workerUpgrade.Id] = workerUpgrade.WorkerCount;
            }

            return saveData;
        }

        // Derived production rates and costs are rebuilt from persistent values.
        public void RestoreFromSaveData(GameSaveData saveData)
        {
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData), "Save data cannot be null.");
            }

            List<string> purchasedActiveUpgradeIds =
                saveData.PurchasedActiveUpgradeIds ?? [];
            Dictionary<string, int> workerUpgradeCounts =
                saveData.WorkerUpgradeCounts ?? [];

            LinesOfCode = Math.Max(0L, saveData.LinesOfCode);
            LifetimeLinesOfCode = Math.Max(0L, saveData.LifetimeLinesOfCode);

            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                bool isPurchased = purchasedActiveUpgradeIds.Contains(upgrade.Id);
                upgrade.RestorePurchaseState(isPurchased);
            }

            LinesPerClick = 1;
            foreach (ActiveUpgrade upgrade in ActiveUpgrades)
            {
                if (upgrade.IsPurchased)
                {
                    LinesPerClick *= upgrade.ClickMultiplier;
                }
            }

            foreach (WorkerUpgrade upgrade in WorkerUpgrades)
            {
                // Missing worker IDs represent a saved count of zero.
                workerUpgradeCounts.TryGetValue(upgrade.Id, out int savedCount);
                upgrade.RestoreWorkerCount(savedCount);
            }

            RecalculateLinesPerSecond();
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
    }
}
