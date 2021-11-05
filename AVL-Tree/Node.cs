using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVL_Tree
{
    public class Node
    {
        public Node Left { get; set; }
        public Node Right { get; set; }
        public int Data { get; set; }
        public int Height { get; set; }
        public Node(int data)
        {
            Data = data;
            Left = Right = null;
            Height = 0;
        }
    }
}
