using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTileSearch {
	Dictionary<Map.Pos,Node> nodes = new Dictionary<Map.Pos,Node>();
    public Unit selectUnit = null; 
    Map map;
    public void Clear()
    {
        selectUnit = null;
    }
    public MovableTileSearch(Map map)
    {        
        this.map = map;       
    }
	class Node{
		public Map.Pos pos;
		public int remindMovePoint;
		//public List<Node> nextNodes = null;
		public Node preNode = null;
        public Node(Map.Pos pos, int remindMovePoint)
        {
            this.pos = pos;
            this.remindMovePoint = remindMovePoint;
        }
        public Node(int x,int y, int remindMovePoint)
        {
            
            this.pos = new Map.Pos(x,y);
            this.remindMovePoint = remindMovePoint;
        }
    }
    public void ClearPos()
    {
        nodes.Clear();
        selectUnit = null;
    }
    public HashSet<Map.Pos> GetMovablePosSet()
    {
        return new HashSet<Map.Pos>(nodes.Keys);
    }
    public bool CheckMovablePos(Map.Pos pos)
    {
        
        return nodes.ContainsKey(pos);
    }
    public List<Map.Pos> GetMovablePosList()
    {
        return new List<Map.Pos>(nodes.Keys);
    }
    public List<Map.Pos> GetMoveLinePos(Map.Pos pos)
    {
        List<Map.Pos> temp = new List<Map.Pos>();
        Node tempNode = nodes[pos];
        while (tempNode!=null)
        {
            temp.Add(tempNode.pos);
            tempNode = tempNode.preNode;
            
        }
        temp.Reverse();
        return temp;
    }
    public List<Map.Pos> GetMoveLinePos(int x, int y)
    {
        return GetMoveLinePos(new Map.Pos(x,y));
    }
    public void SearchTiles(Unit unit){
        
        selectUnit = unit;
		Node zero = new Node(unit.Pos, unit.MovePoint);
        Queue<Node> queue = new Queue<Node>();
        List<Map.Pos> exceptPos = new List<Map.Pos>();
		if (zero.remindMovePoint > 0) {
            IsMovable(zero, queue,unit, exceptPos);
		}

		while(queue.Count>0){
            IsMovable(queue.Dequeue(),queue,unit, exceptPos);
		}

        for(int i=0; i < exceptPos.Count; i++)
        {
            //Debug.Log(exceptPos[i]);
            nodes.Remove(exceptPos[i]);
        }

	}
	void IsMovable(Node node,Queue<Node> queue,Unit unit, List<Map.Pos> exceptPos)
    {
        if (node!=null&&node.remindMovePoint>0)
        {
            Map.Pos pos= node.pos;

            //pos.x+1
            if (pos.x != map.mapXSize - 1)
            {
                AddNode(node,pos.x+1,pos.y,queue,unit,exceptPos);                
            }
            //pos.x-1
            if (pos.x != 0)
            {
                AddNode(node, pos.x - 1, pos.y, queue, unit, exceptPos);
            }
            //pos.y+1
            if (pos.y != map.mapYSize - 1)
            {
                AddNode(node, pos.x , pos.y + 1, queue, unit,  exceptPos);
            }
            //pos.y-1
            if (pos.y != 0)
            {
                AddNode(node, pos.x , pos.y - 1, queue, unit,  exceptPos);
            }
        }        
	}
    void AddNode(Node node,int x,int y,Queue<Node> queue,Unit unit, List<Map.Pos> exceptPos)
    {
        Map.Pos pos = new Map.Pos(x, y);
        if(pos.Equals( unit.Pos))
        {
            return;
        }
        if (nodes.ContainsKey(pos))
        {
            if (node.remindMovePoint <= nodes[pos].preNode.remindMovePoint)
            {
                //ZOC는 나중에 고려
                return;
            }
        }
        if (map.GetPosUnit(x, y) == null && map.GetPosType(x, y) != Map.POSTYPE.NONE)
        {
           
            Map.POSTYPE posType = map.GetPosType(x, y);
                              
            Node nd = new Node(pos, node.remindMovePoint - 1);
            nd.preNode = node;
            nodes.Add(pos, nd);
            queue.Enqueue(nodes[pos]);
            
        }else if (map.GetPosUnit(x, y) != null&&map.GetPosType(x, y) != Map.POSTYPE.NONE)
        {
            Unit unitScript = map.GetPosUnit(x, y).GetComponent<Unit>();
            if (unitScript.identify == unit.identify)
            {
                Map.POSTYPE posType = map.GetPosType(x, y);

                Node nd = new Node(pos, node.remindMovePoint - 1);
                nd.preNode = node;
                nodes.Add(pos, nd);
                queue.Enqueue(nodes[pos]);
                exceptPos.Add(pos);
            }
        }
    }

}
