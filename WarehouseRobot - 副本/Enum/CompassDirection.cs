namespace WarehouseRobot.Enum
{
    /// <summary>
    /// 移动方向
    /// </summary>
    public enum CompassDirections
    {
        /// <summary>
        /// 未设定
        /// </summary>
        NotSet = 0,
        /// <summary>
        /// 上（北）
        /// </summary>
        North = 1,
        /// <summary>
        /// 右上（东北）
        /// </summary>
        NorthEast = 2,
        /// <summary>
        /// 右（东）
        /// </summary>
        East = 3,
        /// <summary>
        /// 右下（东南）
        /// </summary>
        SouthEast = 4,
        /// <summary>
        /// 下（南）
        /// </summary>
        South = 5,
        /// <summary>
        /// 左下（西南）
        /// </summary>
        SouthWest = 6,
        /// <summary>
        /// 左（西）
        /// </summary>
        West = 7,
        /// <summary>
        /// 左上（西北）
        /// </summary>
        NorthWest = 8
    }
}
