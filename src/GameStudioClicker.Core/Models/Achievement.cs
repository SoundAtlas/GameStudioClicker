namespace GameStudioClicker.Core.Models
{
    public class Achievement
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public AchievementRequirementType RequirementType { get; }
        public long RequirementValue { get; }
        public bool IsEarned { get; private set; }
        public bool IsSecret { get; }

        public Achievement(
            string id,
            string displayName,
            string description,
            AchievementRequirementType requirementType,
            long requirementValue)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            RequirementType = requirementType;
            RequirementValue = requirementValue;
            IsEarned = false;
        }

        public void MarkAsEarned()
        {
            IsEarned = true;
        }

        public void RestoreEarnedState(bool isEarned)
        {
            IsEarned = isEarned;
        }
    }
}
