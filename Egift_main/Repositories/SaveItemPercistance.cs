namespace Egift_main.Repositories;

[Serializable]
public class SaveItemPercistance
{
    public static void saveGiftPersistance(string path = "giftSave.xlm")
    {
        StreamWriter fileGift = File.CreateText(path);
    }

}