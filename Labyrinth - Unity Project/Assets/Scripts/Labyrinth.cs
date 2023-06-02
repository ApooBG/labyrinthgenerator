using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Labyrinth : MonoBehaviour
{
    class Cell
    {
        public int cellIndex;
        public GameObject cell;
        public Cell previousCell; //the previous accessed cell by the script

        public GameObject eastWall;
        public GameObject westWall;
        public GameObject northWall;
        public GameObject southWall;

        public Dictionary<string, GameObject> walls;
        public List<Cell> neighbourCells;

        public bool haveVisitedCell; //when the script goes inside this cell, this value will change to true


        public Cell(int cellIndex, GameObject cell, GameObject eastWall, GameObject westWall, GameObject northWall, GameObject southWall)
        {
            neighbourCells = new List<Cell>();
            haveVisitedCell = false;

            this.cellIndex = cellIndex;
            this.cell = cell;
            this.eastWall = eastWall;
            this.westWall = westWall;
            this.northWall = northWall;
            this.southWall = southWall;

            walls = new Dictionary<string, GameObject>();

            AssignWalls();
        }

        public void AssignNeighbours(List<Cell> listOfNeighbours)
        {
            if (neighbourCells.Count == 0)
            {
                neighbourCells = listOfNeighbours;
            }
        }


        void AssignWalls()
        {
            if (eastWall != null)
            {
                walls.Add("eastWall", eastWall);
            }

            if (westWall != null)
            {
                walls.Add("westWall", westWall);
            }

            if (southWall != null)
            {
                walls.Add("southWall", southWall);
            }

            if (northWall != null)
            {
                walls.Add("northWall", northWall);
            }
        }
    }


    [SerializeField] GameObject cell; //a normal cell with south and east walls.

    [SerializeField] GameObject topCell; //a top cell with south, east and north walls
    [SerializeField] GameObject fullCell; //a cell with all the walls


    [SerializeField] int columnNumbers;
    [SerializeField] int rowNumbers;


    List<Cell> allCells; //a list that has all the cells
    List<GameObject> cells; //a list that keeps track of the gameobjects and is modified in the script

    int rowsCreated; //current created rows, used to position the cells
    int currentCellIndex; //in which cell the algorithm currently is


    private void Start()
    {
        allCells = new List<Cell>();
        cells = new List<GameObject>();

        //creating the first cell here
        Cell newCell = new Cell(1, cell, cell.transform.Find("EastWall").gameObject, null, null, cell.transform.Find("SouthWall").gameObject);
        allCells.Add(newCell);
        cells.Add(cell);

        columnNumbers = MainMenu.columnNum;
        rowNumbers = MainMenu.rowsNum;
        currentCellIndex = 1;

        ChangeTheNumberOfRowsAndColumns(rowNumbers, columnNumbers);

        CreateLabyrinth();
    }


    public void ChangeTheNumberOfRowsAndColumns(int numberOfRows, int numberOfColumns)
    {
        if (numberOfRows < 10)
        {
            numberOfRows = 10;
        }

        if (numberOfColumns < 10)
        {
            numberOfColumns = 10;
        }

        if (numberOfRows > 250)
        {
            numberOfRows = 250;
        }

        if (numberOfColumns > 250)
        {
            numberOfColumns = 250;
        }

        this.rowNumbers = numberOfRows;
        this.columnNumbers = numberOfColumns;
    }

    public void CreateLabyrinth()
    {
        if (LabyrinthCreateAllCells())
        {
            BuildLabyrinth(); //after the creation of the cells, this script builds the labyrinth
        }
    }


    bool LabyrinthCreateAllCells()
    {
        do
        {
            CreateCell(); //if the number of cells created is less than the total cells that has to be created, then create a new cell

        } while (rowNumbers * columnNumbers > allCells.Count); //rowNumbers * columnNumbers is the total cells that has to be created

        allCells[0].haveVisitedCell = true; //starting with the first cell

        AssignNeighbours(); //this method is to assign the neighbours in each Cell
        cells.Remove(cell); //removes the first cell from the list

        return true;
    }

    void CreateCell()
    {
        Cell newCell; //create new cell object
        GameObject cloneCell; //create new gameObject 
        GameObject lastCell = cells[cells.Count - 1]; //get the last gameObject cell

        if (cells.Count + 1 == columnNumbers) //top right cell
        {
            cloneCell = Instantiate(topCell);
            newCell = new Cell(cells.Count + 1, cloneCell, null, lastCell.transform.Find("EastWall").gameObject, null, cloneCell.transform.Find("SouthWall").gameObject);
            //newCell = cloneCell.GetComponent<Cell>();
            //newCell.cellIndex = cells.Count + 1;
        }

        else if (cells.Count + 1 <= columnNumbers) //top cell
        {
            cloneCell = Instantiate(topCell);
            newCell = new Cell(cells.Count + 1, cloneCell, cloneCell.transform.Find("EastWall").gameObject, lastCell.transform.Find("EastWall").gameObject, null, cloneCell.transform.Find("SouthWall").gameObject);
        }

        else if (cells.Count % columnNumbers == 0 && cells.Count+1 >= (rowNumbers*columnNumbers) - columnNumbers) //bottom left cell
        {
            cloneCell = Instantiate(fullCell);
            newCell = new Cell(cells.Count + 1, cloneCell, cloneCell.transform.Find("EastWall").gameObject, null, cells[cells.Count - columnNumbers].transform.Find("SouthWall").gameObject, null);
        }

        else if (cells.Count % columnNumbers == 0) //left cell
        {
            cloneCell = Instantiate(fullCell);
            newCell = new Cell(cells.Count + 1, cloneCell, cloneCell.transform.Find("EastWall").gameObject, null, cells[cells.Count - columnNumbers].transform.Find("SouthWall").gameObject, cloneCell.transform.Find("SouthWall").gameObject);
        }


        else if (cells.Count+1 == columnNumbers * rowNumbers) //bottom right cell
        {
            cloneCell = Instantiate(cell);
            newCell = new Cell(cells.Count + 1, cloneCell, cloneCell.transform.Find("EastWall").gameObject, lastCell.transform.Find("EastWall").gameObject, cells[cells.Count - columnNumbers].transform.Find("SouthWall").gameObject, cloneCell.transform.Find("SouthWall").gameObject);
        }

        else if (cells.Count >= (rowNumbers * columnNumbers) - columnNumbers) //bottom cell
        {
            cloneCell = Instantiate(cell);
            newCell = new Cell(cells.Count + 1, cloneCell, cloneCell.transform.Find("EastWall").gameObject, lastCell.transform.Find("EastWall").gameObject, cells[cells.Count - columnNumbers].transform.Find("SouthWall").gameObject, null);
        }

        else if ((cells.Count + 1) % columnNumbers == 0) //right cell
        {
            cloneCell = Instantiate(cell);
            newCell = new Cell(cells.Count + 1, cloneCell, null, lastCell.transform.Find("EastWall").gameObject, cells[cells.Count - columnNumbers].transform.Find("SouthWall").gameObject, cloneCell.transform.Find("SouthWall").gameObject);
        }

        else //a cell that's not on the edge of the labyrinth
        {
            cloneCell = Instantiate(cell);
            newCell = new Cell(cells.Count + 1, cloneCell, cloneCell.transform.Find("EastWall").gameObject, lastCell.transform.Find("EastWall").gameObject, cells[cells.Count - columnNumbers].transform.Find("SouthWall").gameObject, cloneCell.transform.Find("SouthWall").gameObject);
        }

        cells.Add(cloneCell);
        allCells.Add(newCell);
        cloneCell.transform.parent = gameObject.transform; //assign the parent to the new cell gameObject
        cloneCell.name = "Cell" + cells.Count.ToString(); //give it a name with the correct number

        PositionCell(cloneCell, lastCell);
    }

    void PositionCell(GameObject cloneCell, GameObject lastCell)
    {
        if (cells.Count % columnNumbers == 1) //checks if the new cell has to be on a new row
        {
            rowsCreated += 1;

            if (rowsCreated <= rowNumbers)
            {
                cloneCell.transform.position = new Vector3(lastCell.transform.position.x + 1.1144F, lastCell.transform.position.y, cells[0].transform.position.z); //positions it on a new row
            }
        }

        else //if not then positions it next to the last cell
        {
            cloneCell.transform.position = new Vector3(lastCell.transform.position.x, lastCell.transform.position.y, lastCell.transform.position.z + 1f); 
        }
    }


    //foreach cell I check where it is positioned and which cells are it's neighoburs. A cell's neighbours are the ones, that only a wall is standing between them.
    void AssignNeighbours()
    {
        foreach (Cell cell in allCells)
        {
            List<Cell> listOfNeighbours = new List<Cell>(); //creates a list of cells which will be used to assign the neighbours for each cell
            if (cell.cellIndex == columnNumbers) //topRight cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex-1 - 1]); //left neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + columnNumbers]); //bottom neighbour cell

            }

            else if (cell.cellIndex == 1) //topLeft cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + columnNumbers]); //bottom neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + 1]); //right neighbour cell
            }

            else if (cell.cellIndex == columnNumbers * rowNumbers) //bottom right cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - columnNumbers]); //top cell neighbour
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - 1]); //left cell neighbour
            }

            else if (cell.cellIndex % columnNumbers == 1 && cell.cellIndex >= (rowNumbers * columnNumbers) - columnNumbers) //bottom left cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - columnNumbers]); //top neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + 1]); //right neighbour cell
            }

            else if (cell.cellIndex < columnNumbers) //top cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - 1]); //left neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + columnNumbers]); //bottom neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + 1]); //right neighbour cell
            }

            else if (cell.cellIndex % columnNumbers == 1) //left cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + columnNumbers]); //bottom neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - columnNumbers]); //top neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + 1]); //right neighbour cell
            }

            else if (cell.cellIndex > (rowNumbers * columnNumbers) - columnNumbers) //bottom cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - columnNumbers]); //top neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + 1]); //right neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - 1]); //left neighbour cell
            }

            else if (cell.cellIndex % columnNumbers == 0) //right cell
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + columnNumbers]); //bottom neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - 1]); //left neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - columnNumbers]); //top neighbour cell
            }

            else //any other cell meaning it's not on the edge of the labyrinth.
            {
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + columnNumbers]); //bottom neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - 1]); //left neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 - columnNumbers]); //top neighbour cell
                listOfNeighbours.Add(allCells[cell.cellIndex - 1 + 1]); //right neighbour cell
            }

            cell.AssignNeighbours(listOfNeighbours); //gives the list of neighbours to the current cell
        }
    }

    void BuildLabyrinth()
    {
        while (CheckIfThereAreAvailalbeCells()) //checks for unvisited cells
        {
             if (RemoveWallsForCell(currentCellIndex - 1) == false)
             {
                 currentCellIndex = GetPastIndex();
             }
        }

        //comes here after the labyirnth is fully built

        Destroy(allCells[allCells.Count - 1].southWall);
        topCell.SetActive(false); //hide the template of the top cell
        fullCell.SetActive(false); //hide the template of the full cell
    }

    bool CheckIfThereAreAvailalbeCells()
    {
        foreach (Cell cell in allCells)
        {
            if (cell.haveVisitedCell == false) //if there is an unvisited cell, then the labyrinth is not ready
            {
                return true;
            }
        }

        return false;
    }

    bool RemoveWallsForCell(int currentCellIndex)
    {
        Cell currentCell = allCells[currentCellIndex];
        RemoveUnavailableCells(currentCell); //removes the already visited neighbours

        if (currentCell.neighbourCells.Count == 0) //if there are no neighbours, returns false to get another cell
        {
            return false;
        }

        int i = Random.Range(0, currentCell.neighbourCells.Count); //randomly gets a number used to decied to which neighbour it should go
        Cell nextCell = currentCell.neighbourCells[i];

        nextCell.haveVisitedCell = true;

        int indexDifference = nextCell.cellIndex - currentCell.cellIndex; //gets the difference of the index to decide which wall has to be removed
        GameObject wall = null;

        if (indexDifference == 1)
        {
            //went east
            currentCell.walls.TryGetValue("eastWall", out wall);
        }

        else if (indexDifference == -1)
        {
            //went west
            currentCell.walls.TryGetValue("westWall", out wall);
        }

        else if (indexDifference == columnNumbers)
        {
            //went south
            currentCell.walls.TryGetValue("southWall", out wall);
        }

        else if (indexDifference == -columnNumbers)
        {
            //went north
            currentCell.walls.TryGetValue("northWall", out wall);
        }

        nextCell.previousCell = currentCell;

        Destroy(wall);
        this.currentCellIndex = nextCell.cellIndex;

        return true;
    }

    int GetPastIndex()
    {
        return allCells[currentCellIndex-1].previousCell.cellIndex; //gets the index of the previous visited cell
    }


    //foreach neighbour if it has been visited removes it from the list for that cell and is no longer a neighbour.
    void RemoveUnavailableCells(Cell cell)
    {
        List<Cell> listOfCellsToRemove = new List<Cell>();

        foreach (Cell neighbourCell in cell.neighbourCells) 
        {
            if (neighbourCell.haveVisitedCell == true)
            {
                listOfCellsToRemove.Add(neighbourCell);
            }
        }

        foreach (Cell cellToRemove in listOfCellsToRemove) 
        {
            cell.neighbourCells.Remove(cellToRemove);
        }
    }
}

