using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseRobot.Enum;
using WarehouseRobot.Utility;

namespace WarehouseRobot
{
    /// <summary>
    /// AStarRoutePlanner A*路径规划。每个单元格Cell的位置用Point表示
    /// F = G + H 。
    /// G = 从起点A，沿着产生的路径，移动到网格上指定方格的移动耗费。
    /// H = 从网格上那个方格移动到终点B的预估移动耗费。使用曼哈顿方法，它计算从当前格到目的格之间水平和垂直的方格的数量总和，忽略对角线方向。
    /// </summary>
    public class AStarRoutePlanner
    {
        private readonly static Random r = new();
        /// <summary>
        /// 地图宽度（X坐标，水平轴）
        /// </summary>
        public int ColumnCount { get; }
        /// <summary>
        /// 地图高度(Y坐标，竖直轴)
        /// </summary>
        public int RowCount { get; }
        /// <summary>
        /// 计算移动花费
        /// </summary>
        private readonly ICostGetter costGetter = new SimpleCostGetter();
        /// <summary>
        /// 地图
        /// </summary>
        public ZoneState[,] Grid { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="columnCount"></param>
        /// <param name="rowCount"></param>
        /// <param name="costGetter"></param>
        public AStarRoutePlanner(int columnCount, int rowCount, ICostGetter costGetter)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            this.costGetter = costGetter;

            Initialize();
        }

        /// <summary>
        /// 将所有位置进行随机初始化。
        /// </summary>
        public void Initialize()
        {
            Grid = new ZoneState[RowCount, ColumnCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    if (r.Next() % 10 == 0)
                        Grid[i, j] = ZoneState.Blocked;
                    else
                        Grid[i, j] = ZoneState.Empty;
                }
            }
        }

        /// <summary>
        /// 指定障碍物位置，进行初始化。
        /// </summary>
        /// <param name="obstaclePoints">指定障碍物的位置</param>
        public void Initialize(IList<Point> obstaclePoints)
        {
            Grid = new ZoneState[RowCount, ColumnCount];

            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    Grid[i, j] = ZoneState.Empty;
                }
            }
            foreach (Point pt in obstaclePoints)
            {
                Grid[pt.Y, pt.X] = ZoneState.Blocked;
            }
        }

        /// <summary>
        /// 制作从起点到终点的计划
        /// </summary>
        /// <param name="start">起点</param>
        /// <param name="destination">终点</param>
        /// <returns>返回路径</returns>
        public IList<Point> Plan(Point start, Point destination)
        {
            Rectangle map = new(0, 0, ColumnCount, RowCount);
            if (!map.Contains(start))
            {
                throw new ArgumentOutOfRangeException(nameof(start), start, "StartPoint not in current map.");
            }
            if (!map.Contains(destination))
            {
                throw new ArgumentOutOfRangeException(nameof(destination), destination, "Destination not in current map.");
            }

            RoutePlanData routePlanData = new(map, destination);

            AStarNode startNode = new(start, null, 0, 0);
            routePlanData.OpenedList.Add(startNode);

            AStarNode currenNode = startNode;

            //从起始节点开始进行递归调用
            return DoPlan(routePlanData, currenNode);
        }

        private IList<Point> DoPlan(RoutePlanData routePlanData, AStarNode currenNode)
        {
            IList<CompassDirections> allCompassDirections = CompassDirectionsHelper.GetReasonableCompassDirections();
            foreach (CompassDirections direction in allCompassDirections)
            {
                Point nextCell = GeometryHelper.GetAdjacentPoint(currenNode.Location, direction);
                if (!routePlanData.CellMap.Contains(nextCell)) 
                {
                    // 相邻点已经在地图之外
                    continue;
                }

                if (Grid[nextCell.Y, nextCell.X] == ZoneState.Blocked)
                {
                    // 下一个Cell为障碍物
                    continue;
                }

                int costG = costGetter.GetCost(currenNode.Location, direction);
                int costH = Math.Abs(nextCell.X - routePlanData.Destination.X) + Math.Abs(nextCell.Y - routePlanData.Destination.Y);
                if (costH == 0) 
                {
                    // costH为0，表示相邻点就是目的点，规划完成，构造结果路径
                    IList<Point> route = new List<Point>();
                    route.Add(routePlanData.Destination);
                    route.Insert(0, currenNode.Location);
                    AStarNode tempNode = currenNode;
                    while (tempNode.PreviousNode != null)
                    {
                        route.Insert(0, tempNode.PreviousNode.Location);
                        tempNode = tempNode.PreviousNode;
                    }

                    return route;
                }

                AStarNode existNode = GetNodeOnLocation(nextCell, routePlanData);
                if (existNode != null)
                {
                    if (existNode.CostG > costG)
                    {
                        //如果新的路径代价更小，则更新该位置上的节点的原始路径
                        existNode.ResetPreviousNode(currenNode, costG);
                    }
                }
                else
                {
                    AStarNode newNode = new(nextCell, currenNode, costG, costH);
                    routePlanData.OpenedList.Add(newNode);
                }
            }

            //将已遍历过的节点从开放列表转移到关闭列表
            routePlanData.OpenedList.Remove(currenNode);
            routePlanData.ClosedList.Add(currenNode);

            AStarNode minCostNode = GetMinCostNode(routePlanData.OpenedList);
            if (minCostNode == null) //表明从起点到终点之间没有任何通路。
            {
                return null;
            }

            //对开放列表中的下一个代价最小的节点作递归调用
            return this.DoPlan(routePlanData, minCostNode);
        }

        /// <summary>
        /// 目标位置location是否已存在于开放列表或关闭列表中
        /// </summary>       
        private AStarNode GetNodeOnLocation(Point location, RoutePlanData routePlanData)
        {
            foreach (AStarNode temp in routePlanData.OpenedList)
            {
                if (temp.Location == location)
                {
                    return temp;
                }
            }

            foreach (AStarNode temp in routePlanData.ClosedList)
            {
                if (temp.Location == location)
                {
                    return temp;
                }
            }

            return null;
        }

        /// <summary>
        /// 从开放列表中获取代价F最小的节点，以启动下一次递归
        /// </summary>      
        private AStarNode GetMinCostNode(IList<AStarNode> openedList)
        {
            if (openedList.Count == 0)
            {
                return null;
            }

            AStarNode target = openedList[0];
            foreach (AStarNode temp in openedList)
            {
                if (temp.CostF < target.CostF)
                {
                    target = temp;
                }
            }

            return target;
        }
    }
}
