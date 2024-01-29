
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public int Coins = 0;
        public int Level = 1;
        public int KickLevel = 1;
        public int ScoreLevel = 1;
        public int SpeedLevel = 1;
        public int Highscore = 0;
        public int SkinIndex = 0;
        public bool IsSoundEnabled = true;
        public bool IsMusicEnabled = true;
        public int skinToUnlockId = 0;
        public float skinUnlockProgress = 0f;
        public bool[] unlockSkins = new bool[12];

        public SavesYG()
        {
            unlockSkins[0] = true;
        }
    }
}
