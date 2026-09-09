namespace GameStudioClicker.Core.Persistence
{
    public interface IGameSaveRepository
    {
        GameSaveData? Load();
        void Save(GameSaveData saveData);
    }
}
