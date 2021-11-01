using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;

namespace WarehouseRobot
{
    /// <summary>
    /// 一次路径规划过程中的规划信息。
    /// </summary>
    public class RoutePlanData
    {
        /// <summary>
        /// 地图的矩形大小。经过单元格标准处理。
        /// </summary>
        public Rectangle CellMap
        {
            get;
        }

        /// <summary>
        /// ClosedList 关闭列表，即存放已经遍历处理过的节点。
        /// </summary>
        public IList<AStarNode> ClosedList
        {
            get;
        } = new List<AStarNode>();

        /// <summary>
        /// OpenedList 开放列表，即存放已经开发但是还未处理的节点。
        /// </summary>
        public IList<AStarNode> OpenedList
        {
            get;
        } = new List<AStarNode>();

        /// <summary>
        /// Destination 目的节点的位置。
        /// </summary>
        public RHPoint Destination
        {
            get;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="map"></param>
        /// <param name="destination"></param>
        public RoutePlanData(Rectangle map, RHPoint destination)
        {
            CellMap = map;
            Destination = destination;
        }
    }
}
