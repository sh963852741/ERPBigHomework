
using System.Drawing;

namespace WarehouseRobot
{
    /// <summary>
    /// 保存规划到当前节点时的各个Cost值以及父节点。
    /// </summary>
    public class AStarNode
    {
        /// <summary>
        /// 节点所在的位置，其X值代表ColumnIndex，Y值代表LineIndex
        /// </summary>
        public Point Location
        {
            get;
            private set;
        } = new Point(0, 0);
        /// <summary>
        /// 父节点，即是由哪个节点导航到当前节点的。
        /// </summary>
        public AStarNode PreviousNode
        {
            get;
            private set;
        }
        /// <summary>
        /// CostF 从起点导航经过本节点然后再到目的节点的估算总代价。
        /// </summary>
        public int CostF
        {
            get
            {
                return CostG + CostH;
            }
        }
        /// <summary>
        /// CostG 从起点导航到本节点的代价。
        /// </summary>
        public int CostG
        {
            get;
            private set;
        }
        /// <summary>
        /// CostH 使用启发式方法估算的从本节点到目的节点的代价。
        /// </summary>
        public int CostH
        {
            get;
            private set;
        }

        public AStarNode(Point location, AStarNode previous, int costG, int costH)
        {
            Location = location;
            PreviousNode = previous;
            CostG = costG;
            CostH = costH;
        }

        /// <summary>
        /// 当从起点到达本节点有更优的路径时，调用该方法采用更优的路径。
        /// </summary>        
        public void ResetPreviousNode(AStarNode previous, int costG)
        {
            PreviousNode = previous;
            CostG = costG;
        }

        public override string ToString()
        {
            return Location.ToString();
        }
    }
}