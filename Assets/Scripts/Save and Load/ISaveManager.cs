public interface ISaveManager
{
    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="_data">数据</param>
    void LoadData(GameData _data);
    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="_data">数据</param>
    void SaveData(ref GameData _data);
}
