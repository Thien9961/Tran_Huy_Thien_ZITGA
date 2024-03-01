
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ketra
{
    public class MazeGenerator : Singleton<MazeGenerator>
    {
        [SerializeField]
        private MazeCell _mazeCellPrefab;

        [SerializeField]
        private int _mazeWidth;

        [SerializeField]
        private int _mazeDepth;

        public MazeCell[,] _mazeGrid;

        public Vector2 origin;
        public RenderTexture overview;
        public string pathColor;
        public List<GameObject> path = new List<GameObject>();
        public int index { get { return int.Parse(name); } private set => name = value.ToString(); }

        private void Start()
        {
            
        }

        public void Generate(int index)
        {
            this.index = index;
            _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeDepth; z++)
                {
                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector2(x, z) + origin, Quaternion.identity);
                    _mazeGrid[x, z].transform.parent = transform;
                    _mazeGrid[x, z].UnVisit();
                }
            }
            var bug = Instantiate(Resources.Load<Bug>("Bug"), _mazeGrid[0, _mazeDepth - 1].transform);
            Camera cam = new GameObject("Camera").AddComponent<Camera>();
            cam.transform.parent = transform;
            cam.transform.position = Vector3.right * 4.5f + Vector3.up * 6.75f + Vector3.back * 10;
            cam.targetTexture = overview;
            cam.orthographic = true;
            cam.orthographicSize = 11;
            cam.cullingMask = 1 << 4;
            path.Add(_mazeGrid[0, _mazeDepth - 1].gameObject);
            GenerateMaze(null, _mazeGrid[0, 0]);

        }

        public void ShowPath()
        {
            for (int i = 0; i < path.Count; i++)
            {
                Color color;
                ColorUtility.TryParseHtmlString("#" + pathColor, out color);
                path[i].GetComponent<SpriteRenderer>().color = color;
            }
        }

        private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
        {
            currentCell.Visit();
            ClearWalls(previousCell, currentCell);
            MazeCell nextCell;
            do
            {
                nextCell = GetNextUnvisitedCell(currentCell);
                
                if (nextCell != null)
                {
                    GenerateMaze(currentCell, nextCell);
                }
                    
            } while (nextCell != null);
            if (Target.instance == null)
                if (((currentCell == _mazeGrid[1, _mazeDepth - 1] && currentCell.transform.GetChild(0).gameObject.activeSelf == false)) || ((currentCell == _mazeGrid[0, _mazeDepth - 2]) && (currentCell.transform.GetChild(2).gameObject.activeSelf == false)))
                {
                    path.Add(currentCell.gameObject);
                    Instantiate(Resources.Load<Target>("Target"), currentCell.transform);
                }
        }

        private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
        {
            var unvisitedCells = GetUnvisitedCells(currentCell);

            return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
        }

        private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
        {
            int x = (int)currentCell.transform.position.x;
            int z = (int)currentCell.transform.position.y;
            if (x + 1 < _mazeWidth)
            {
                var cellToRight = _mazeGrid[x + 1, z];

                if (cellToRight.IsVisited == false)
                {
                    yield return cellToRight;
                }
            }
            if (x - 1 >= 0)
            {

                var cellToLeft = _mazeGrid[x-1, z];
                if (cellToLeft.IsVisited == false)
                {
                    yield return cellToLeft;
                }
            }

            if (z + 1 < _mazeDepth)
            {
                var cellToFront = _mazeGrid[x, z + 1];

                if (cellToFront.IsVisited == false)
                {
                    yield return cellToFront;
                }
            }

            if (z - 1 >= 0)
            {
                var cellToBack = _mazeGrid[x, z - 1];

                if (cellToBack.IsVisited == false)
                {
                    yield return cellToBack;
                }
            }
        }

        private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
        {
            if (previousCell == null)
            {
                return;
            }

            if (previousCell.transform.position.x < currentCell.transform.position.x)
            {
                previousCell.ClearRightWall();
                currentCell.ClearLeftWall();
                return;
            }

            if (previousCell.transform.position.x > currentCell.transform.position.x)
            {
                previousCell.ClearLeftWall();
                currentCell.ClearRightWall();
                return;
            }

            if (previousCell.transform.position.y < currentCell.transform.position.y)
            {
                previousCell.ClearFrontWall();
                currentCell.ClearBackWall();
                return;
            }

            if (previousCell.transform.position.y > currentCell.transform.position.y)
            {
                previousCell.ClearBackWall();
                currentCell.ClearFrontWall();
                return;
            }
        }

    }



}

