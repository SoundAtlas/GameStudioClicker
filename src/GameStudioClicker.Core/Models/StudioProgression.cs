namespace GameStudioClicker.Core.Models
{
    public class StudioProgression
    {
        private readonly static long[] _levelThresholds = { 0, 100, 250, 500, 900 };
        public long TotalExperience { get; private set; } = 0;
        public int Level
        {
            get
            {
                int level = 1;
                for (int i = 1; i < _levelThresholds.Length; i++)
                {
                    if (TotalExperience >= _levelThresholds[i])
                    {
                        level = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                return level;
            }
        }

        public long ExperienceIntoCurrentLevel
        {
            get
            {
                if (Level >= _levelThresholds.Length)
                {
                    return 0;
                }

                return TotalExperience - _levelThresholds[Level - 1];
            }

        }
        public long ExperienceNeededForNextLevel
        {
            get
            {
                if (Level >= _levelThresholds.Length)
                {
                    return 0;
                }

                return _levelThresholds[Level] - _levelThresholds[Level - 1];
            }
        }

        public void AddExperience(long experience)
        {
            if (experience > 0)
            {
                TotalExperience += experience;
            }
        }

        public void RestoreExperience(long experience)
        {
            TotalExperience = Math.Clamp(experience, 0, long.MaxValue);
        }
    }
}
