using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    class EndPoint
    {
        public Box Box;
        public bool IsStart;
        public float Value;
    }

    class Box
    {
        public EndPoint Start;
        public EndPoint End;
        public IBoundable GameObject;
    }
    public class AxisList
    {
        Dictionary<IBoundable, Box> boxes = new Dictionary<IBoundable, Box>();

        /// <summary>
        /// The list of endpoints --> the axis list
        /// </summary>
        List<EndPoint> endPoints = new List<EndPoint>();

        public void AddGameObject(IBoundable gameObject)
        {
            var box = new Box()
            {
                GameObject = gameObject
            };
            EndPoint start = new EndPoint()
            {
                Box = box,
                IsStart = true,
                Value = gameObject.Bounds.X
            };
            box.Start = start;
            EndPoint end = new EndPoint()
            {
                Box = box,
                IsStart = false,
                Value = gameObject.Bounds.X + gameObject.Bounds.Width
            };
            box.End = end;
            boxes.Add(gameObject, box);
            endPoints.Add(start);
            endPoints.Add(end);
            Sort();
        }

        public void UpdateGameObject(IBoundable gameObject)
        {
            var box = boxes[gameObject];
            box.Start.Value = gameObject.Bounds.X;
            box.End.Value = gameObject.Bounds.X + gameObject.Bounds.Width;
            Sort();
        }

        void Sort()
        {
            int i = 1;
            while (i < endPoints.Count)
            {
                int j = 1;
                while (j > 0 && endPoints[j-1].Value > endPoints[j].Value)
                {
                    var tmp = endPoints[j - 1];
                    endPoints[j - 1] = endPoints[j];
                    endPoints[j] = tmp;
                }
                i++;
            }
        }

        public IEnumerable<IBoundable> QueryRange(float start, float end)
        {
            List<IBoundable> open = new List<IBoundable>();
            foreach(var point in endPoints)
            {
                if (point.Value > end)
                {
                    break;
                }
                if (point.IsStart)
                {
                    open.Add(point.Box.GameObject);
                }
                else if (point.Value < start)
                {
                    open.Remove(point.Box.GameObject);
                }
            }
            return open;
        }

        public IEnumerable<Tuple<IBoundable, IBoundable>> GetCollisionPairs()
        {
            List<IBoundable> open = new List<IBoundable>();
            List<Tuple<IBoundable, IBoundable>> pairs = new List<Tuple<IBoundable, IBoundable>>();
            foreach (EndPoint point in endPoints)
            {
                if (point.IsStart)
                {
                    foreach (var other in open)
                    {
                        pairs.Add(new Tuple<IBoundable, IBoundable>(point.Box.GameObject, other));
                    }
                    open.Add(point.Box.GameObject);
                }
                else
                {
                    open.Remove(point.Box.GameObject);
                }
            }
            return pairs;
        }

    }
}
