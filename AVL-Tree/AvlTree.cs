using System;

namespace AVL_Tree
{
    public class AvlTree
    {
        public Node Root { get; set; }
        public int Count { get; private set; }
        System.Windows.Forms.Form Form { get; set; } = new System.Windows.Forms.Form();
        //create a viewer object 
        Microsoft.Msagl.GraphViewerGdi.GViewer Viewer { get; set; } = new Microsoft.Msagl.GraphViewerGdi.GViewer();
        //create a graph object 
        Microsoft.Msagl.Drawing.Graph Graph { get; set; } = new Microsoft.Msagl.Drawing.Graph("graph");
        public AvlTree(int data)
        {
            Root = new Node(data);
            Count = 1;
        }
        public AvlTree() : this(0) { }
        public bool Contains(Node node, int data)
        {
            if (node == null) return false;
            if (node.Data == data) return true;
            if (node.Data < data) return Contains(node.Right, data);
            return Contains(node.Left, data);
        }
        public int MinElement(Node node)
        {
            if (node.Left == null) return node.Data;
            return MinElement(node.Left);
        }
        public int MaxElement(Node node)
        {
            if (node.Right == null) return node.Data;
            return MaxElement(node.Right);
        }
        public Node Remove(Node node, int data)
        {
            if (node == Root)
                Count--;
            if (node == null)
            {
                Count++;
                return null;
            }
            if (data < node.Data)
            {
                node.Left = Remove(node.Left, data);
            }
            else if (data > node.Data)
            {
                node.Right = Remove(node.Right, data);
            }
            else
            {
                //node without or with one children
                if (node.Left == null || node.Right == null)
                {
                    Node temp = null;
                    if (temp == node.Left)
                        temp = node.Right;
                    else
                        temp = node.Left;

                    if (temp == null)
                    {
                        temp = node;
                        node = null;
                    }
                    else
                        node = temp;
                }
                // node with two children
                else
                {
                    int rightSubtreeMinValue = MinElement(node.Right);
                    node.Data = rightSubtreeMinValue; // set the data
                    node.Right = Remove(node.Right, rightSubtreeMinValue); // remove leaf
                }
            }
            FixHeight(node);
            return Balance(node);
        }
        private bool IsLeaf(Node node)
        {
            return node.Left == null && node.Right == null;
        }
        public Node Add(Node node, int data)
        {
            if (data < node.Data)
            {
                if (node.Left == null)
                {
                    node.Left = new Node(data);
                    Count++;
                }
                else
                {
                    node.Left = Add(node.Left, data);
                }
            }
            else if (data > node.Data)
            {
                if (node.Right == null)
                {
                    node.Right = new Node(data);
                    Count++;
                }
                else
                {
                    node.Right = Add(node.Right, data);
                }
            }
            FixHeight(node);
            return Balance(node);
        }
        private Node Balance(Node node)
        {
            if (node == null) return null;
            int balance = GetBalanceFactor(node);

            // left overweight
            if (balance > 1)
            {
                if (GetBalanceFactor(node.Left) < 0)
                {
                    node.Left = RotateRight(node.Left);
                }
                return RotateLeft(node);
            }
            // right overweight
            if (balance < -1)
            {
                if (GetBalanceFactor(node.Right) > 0)
                {
                    node.Right = RotateLeft(node.Right);
                }
                return RotateRight(node);
            }
            return node;
        }
        private Node RotateRight(Node x)
        {
            Node y = x.Right;
            x.Right = y.Left;
            y.Left = x;

            FixHeight(x);
            FixHeight(y);

            if (x == Root) Root = y;
            return y;
        }
        private Node RotateLeft(Node x)
        {
            Node y = x.Left;
            x.Left = y.Right;
            y.Right = x;

            FixHeight(x);
            FixHeight(y);

            if (x == Root) Root = y;
            return y;
        }
        private int Height(Node node)
        {
            if (node == null) return -1;
            return node.Height;
        }
        private void FixHeight(Node node)
        {
            if (node != null)
                node.Height = Math.Max(Height(node.Left), Height(node.Right)) + 1;
        }
        private int GetBalanceFactor(Node node)
        {
            if (node == null)
                return 0;
            return Height(node.Left) - Height(node.Right);
        }
        private void GetGraph(Node node)
        {
            if (node == null) return;

            string right = node?.Right?.Data.ToString() ?? "null";
            string left = node?.Left?.Data.ToString() ?? "null";
            Graph.AddEdge($"{node.Data}", left);
            Graph.AddEdge($"{node.Data}", right);
            GetGraph(node.Left);
            GetGraph(node.Right);
        }
        public void ShowTree()
        {
            Graph = new Microsoft.Msagl.Drawing.Graph("graph");
            GetGraph(Root);
            //bind the graph to the viewer 
            Viewer.Graph = Graph;
            //associate the viewer with the form 
            Form.SuspendLayout();
            Viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            Form.Controls.Add(Viewer);
            Form.ResumeLayout();
            //show the form 
            Form.ShowDialog();
        }
        public string PreOrderString { get; private set; } = String.Empty;
        public string InOrderString { get; private set; } = String.Empty;
        public string PostOrderString { get; private set; } = String.Empty;
        public void PreOrder(Node node)
        {
            if (node == Root) PreOrderString = String.Empty;
            if (node == null) return;
            PreOrderString += $"{node.Data} ";
            PreOrder(node.Left);
            PreOrder(node.Right);
        }
        public void InOrder(Node node)
        {
            if (node == Root) InOrderString = String.Empty;
            if (node == null) return;
            InOrder(node.Left);
            InOrderString += $"{node.Data} ";
            InOrder(node.Right);
        }
        public void PostOrder(Node node)
        {
            if (node == Root) PostOrderString = String.Empty;
            if (node == null) return;
            PostOrder(node.Left);
            PostOrder(node.Right);
            PostOrderString += $"{node.Data} ";
        }
    }
}
