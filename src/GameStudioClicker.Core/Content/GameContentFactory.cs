using GameStudioClicker.Core.Models;

namespace GameStudioClicker.Core.Content
{
    internal static class GameContentFactory
    {
        internal static IReadOnlyList<ActiveUpgrade> CreateActiveUpgrades()
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
                cost: 12_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "junior_developer",
                prerequisite: developerLaptop);

            ActiveUpgrade professionalIdeLicense = new ActiveUpgrade(
                id: "professional_ide_license",
                displayName: "Professional IDE License",
                description: "Doubles Lines of Code / Click",
                cost: 25_000,
                clickMultiplier: 2,
                prerequisite: developerLaptop);

            ActiveUpgrade highEndWorkstation = new ActiveUpgrade(
                id: "high_end_workstation",
                displayName: "High-End Workstation",
                description: "Doubles Lines of Code / Click",
                cost: 50_000,
                clickMultiplier: 2,
                prerequisite: professionalIdeLicense);

            ActiveUpgrade internMentorshipProgram = new ActiveUpgrade(
                id: "intern_mentorship_program",
                displayName: "Intern Mentorship Program",
                description: "Triples Intern Productivity",
                cost: 100_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "intern",
                prerequisite: highEndWorkstation);

            ActiveUpgrade buildServer = new ActiveUpgrade(
                id: "build_server",
                displayName: "Build Server",
                description: "Doubles Lines of Code / Click",
                cost: 150_000,
                clickMultiplier: 2,
                prerequisite: highEndWorkstation);

            ActiveUpgrade automatedTestingSuite = new ActiveUpgrade(
                id: "automated_testing_suite",
                displayName: "Automated Testing Suite",
                description: "Triples Lines of Code / Click",
                cost: 200_000,
                clickMultiplier: 3,
                prerequisite: buildServer);

            ActiveUpgrade architectureWorkshop = new ActiveUpgrade(
                id: "architecture_workshop",
                displayName: "Architecture Workshop",
                description: "Doubles Senior Developer Productivity",
                cost: 250_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "senior_developer",
                prerequisite: automatedTestingSuite);



            // Era 3: Established Game Studio  
            ActiveUpgrade automatedDevelopmentPipeline = new ActiveUpgrade(
                id: "automated_development_pipeline",
                displayName: "Automated Development Pipeline",
                description: "4x All Worker Productivity",
                cost: 350_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 4,
                targetAllWorkers: true,
                prerequisite: automatedTestingSuite);

            ActiveUpgrade captureStudio = new ActiveUpgrade(
                id: "capture_studio",
                displayName: "Motion-Capture Studio",
                description: "Doubles Lines of Code / Click",
                cost: 400_000,
                clickMultiplier: 2,
                prerequisite: automatedDevelopmentPipeline);

            ActiveUpgrade pairProgrammingSessions = new ActiveUpgrade(
                id: "pair_programming_sessions",
                displayName: "Pair Programming Sessions",
                description: "Triples Junior Developer Productivity",
                cost: 750_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "junior_developer",
                prerequisite: captureStudio);

            ActiveUpgrade studioServerRack = new ActiveUpgrade(
                id: "studio_server_rack",
                displayName: "Studio Server Rack",
                description: "Doubles Lines of Code / Click",
                cost: 1_000_000,
                clickMultiplier: 2,
                prerequisite: captureStudio);

            ActiveUpgrade proprietaryGameEngine = new ActiveUpgrade(
                id: "proprietary_game_engine",
                displayName: "Proprietary Game Engine",
                description: "Doubles Lines of Code / Click",
                cost: 1_500_000,
                clickMultiplier: 2,
                prerequisite: studioServerRack);

            ActiveUpgrade technicalLeadershipTraining = new ActiveUpgrade(
                id: "technical_leadership_training",
                displayName: "Technical Leadership Training",
                description: "Doubles Lead Developer Productivity",
                cost: 2_000_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "lead_developer",
                prerequisite: proprietaryGameEngine);

            ActiveUpgrade renderFarm = new ActiveUpgrade(
                id: "render_farm",
                displayName: "Render Farm",
                description: "Doubles Lines of Code / Click",
                cost: 2_500_000,
                clickMultiplier: 2,
                prerequisite: proprietaryGameEngine);

            ActiveUpgrade globalCloudInfrastructure = new ActiveUpgrade(
                id: "global_cloud_infrastructure",
                displayName: "Global Cloud Infrastructure",
                description: "Triples Lines of Code / Click",
                cost: 3_000_000,
                clickMultiplier: 3,
                prerequisite: renderFarm);


            // Era 4 Cutting-Edge Megastudio

            ActiveUpgrade aiWorkstations = new ActiveUpgrade(
                id: "ai_workstations",
                displayName: "AI Workstations",
                description: "4x Lines of Code / Click",
                cost: 3_500_000,
                clickMultiplier: 4,
                prerequisite: globalCloudInfrastructure);

            ActiveUpgrade developerToolkit = new ActiveUpgrade(
                id: "developer_toolkit",
                displayName: "Proprietary Developer Toolkit",
                description: "Doubles Senior Developer Productivity",
                cost: 5_000_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "senior_developer",
                prerequisite: aiWorkstations);

            ActiveUpgrade neuralMotionCaptureSystem = new ActiveUpgrade(
                id: "neural_motion_capture_system",
                displayName: "Neural Motion-Capture System",
                description: "Doubles Lines of Code / Click",
                cost: 7_500_000,
                clickMultiplier: 2,
                prerequisite: aiWorkstations);

            ActiveUpgrade autonomousQaSwarm = new ActiveUpgrade(
                id: "autonomous_qa_swarm",
                displayName: "Autonomous QA Swarm",
                description: "Doubles Lines of Code / Click",
                cost: 10_000_000,
                clickMultiplier: 2,
                prerequisite: neuralMotionCaptureSystem);

            ActiveUpgrade selfOrganizingDevTeams = new ActiveUpgrade(
                id: "self_organizing_dev_teams",
                displayName: "Self-Organizing Dev Teams",
                description: "Doubles Lead Developer Productivity",
                cost: 12_500_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 2,
                targetWorkerId: "lead_developer",
                prerequisite: autonomousQaSwarm);

            ActiveUpgrade quantumBuildServer = new ActiveUpgrade(
                id: "quantum_build_server",
                displayName: "Quantum Build Server",
                description: "Doubles Lines of Code / Click",
                cost: 15_000_000,
                clickMultiplier: 2,
                prerequisite: autonomousQaSwarm);

            ActiveUpgrade predictiveGameEngine = new ActiveUpgrade(
                id: "predictive_game_engine",
                displayName: "Predictive Game Engine",
                description: "Triples Lines of Code / Click",
                cost: 20_000_000,
                clickMultiplier: 3,
                prerequisite: quantumBuildServer);

            ActiveUpgrade adaptiveLearningProgram = new ActiveUpgrade(
                id: "adaptive_learning_program",
                displayName: "Adaptive Learning Program",
                description: "Triples Intern Productivity",
                cost: 50_000_000,
                clickMultiplier: 1,
                workerProductionMultiplier: 3,
                targetWorkerId: "intern",
                prerequisite: predictiveGameEngine);

            // Additional hardware upgrades can extend this ordered progression.
            return new List<ActiveUpgrade>
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
                captureStudio,
                pairProgrammingSessions,
                studioServerRack,
                proprietaryGameEngine,
                technicalLeadershipTraining,
                renderFarm,
                globalCloudInfrastructure,

                aiWorkstations,
                developerToolkit,
                neuralMotionCaptureSystem,
                autonomousQaSwarm,
                selfOrganizingDevTeams,
                quantumBuildServer,
                predictiveGameEngine,
                adaptiveLearningProgram
            };
        }

        internal static IReadOnlyList<WorkerUpgrade> CreateWorkerUpgrades()
        {
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

            return new List<WorkerUpgrade>
            {
                intern,
                juniorDeveloper,
                seniorDeveloper,
                leadDeveloper
            };
        }
    }
}
