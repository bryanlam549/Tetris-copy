using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    //Grid to check what space been taken up by blocks
    public static int grid_Width = 12;
    public static int grid_Height = 23;
    public static bool[,] grid = new bool[grid_Width, grid_Height];
    
    //HUD stuff
    public RectTransform mPanelGameover;
    public TextMeshProUGUI level_Text;
    public TextMeshProUGUI line_Count_Text;
    public int difficulty_level = 1;
    private int line_Count = 0;

    //Control of fall speed, decreases with difficulty
    public float fall_speed = 1.0f;
    
    //Keep track of next, current and held tetromino
    private GameObject nextTetromino;
    private GameObject currentTetromino;
    private GameObject holdTetromino;

    //Sounds for completing rows
    public AudioClip complete_Row;
    public AudioClip complete_Row4;
    private AudioSource audio_Source;

    // Start is called before the first frame update
    void Start()
    {
        audio_Source = GetComponent<AudioSource>();
        //Soawn tetromino
        LoadNextTetro();
        SpawnNextTetro();
        //Instantiate grid array
        for (int y = 0; y < grid_Height; y++)
        {
            for (int x = 0; x < grid_Width; x++)
            {
                grid[x, y] = false;
            }
        }
        //Make the ground true
        for (int x = 0; x < grid_Width; x++)
        {
            grid[x, 0] = true;
        }
        //Make the walls true
        for (int y = 0; y < grid_Height; y++)
        {
            grid[0, y] = true;
            grid[11, y] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    //Hold and spawn tetrominos
    public void HoldTetro()
    {
        if(holdTetromino != null)
        {

            GameObject tempObject;
            currentTetromino.transform.position = new Vector3(14f, 3.44f);
            holdTetromino.transform.position = new Vector3(5.0f, 21.0f);
            tempObject = holdTetromino;

            holdTetromino = currentTetromino;
            holdTetromino.GetComponent<Tetromino>().enabled = false;
            currentTetromino = tempObject;
            currentTetromino.GetComponent<Tetromino>().enabled = true;


        }
        else
        {
            currentTetromino.transform.position = new Vector3(14f, 3.44f);
            holdTetromino = currentTetromino;
            holdTetromino.GetComponent<Tetromino>().enabled = false;
            SpawnNextTetro();
        }
    }
    public void LoadNextTetro()
    {
        int randomNum = Random.Range(0, 7);
        float xCoor = -3f;
        float yCoor = 3.44f;

        switch (randomNum)
        {
            case 0:
                //yCoor = 2.44f;
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_t", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
                
            case 1:
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_J", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
            case 2:
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_L", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
            case 3:
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_long", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
            case 4:
                xCoor = -3.5f;
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_square", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
            case 5:
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_s", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
            case 6:
                nextTetromino = (GameObject)Instantiate(Resources.Load(
                    "Prefabs/Tetromino_z", typeof(GameObject)), new Vector2(xCoor, yCoor), Quaternion.identity);
                goto default;
            default:
                nextTetromino.GetComponent<Tetromino>().enabled = false;
                
                break;
        }
    }
    public void SpawnNextTetro()
    {
        nextTetromino.transform.position = new Vector3(5.0f, 21.0f);
        currentTetromino = nextTetromino;
        currentTetromino.GetComponent<Tetromino>().enabled = true;
        LoadNextTetro();
    }

    //Checks if a block is in the grid
    public bool inGrid(Transform mino)
    {
        if (grid[(int)Mathf.Round(mino.position.x), (int)Mathf.Round(mino.position.y)] == true)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //Updates a grid whenever block lands
    public void UpdateGrid(Tetromino tetro)
    {
        //Iterate through each mino and update the grid
        foreach (Transform mino in tetro.transform)
        {
            grid[(int)Mathf.Round(mino.position.x), (int)Mathf.Round(mino.position.y)] = true;
            mino.tag = ((int)Mathf.Round(mino.position.y)).ToString();
        }
    }

    //Whenever row is full, delete the row, move all rows down
    public void deleteRow(Tetromino tetro)
    {
        #region failed attempt
        /*
        var check_Row_Set = new HashSet<int>();
        foreach (Transform mino in tetro.transform)
        {
            check_Row_Set.Add((int)Mathf.Round(mino.position.y));
        }
        
        foreach (int rowNum in check_Row_Set)
        {
            //If the row is full...
            if (GameObject.FindGameObjectsWithTag(rowNum.ToString()).Length == 10)
            {
                //Then delete all items on that row and make the row all false
                foreach (GameObject gos in GameObject.FindGameObjectsWithTag(rowNum.ToString()))
                {
                    Destroy(gos);
                }
                for(int x = 1; x < grid_Width-1; x++)
                {
                    grid[x, rowNum] = false;
                }
                //Then move all rows above it down
                MoveAllRowsDown((int)Mathf.Round(rowNum + 1));
                
            }
        }*/
        #endregion        
        int score_Mul = 0;
        for (int y = grid_Height-1; y > 0; y--)
        {
            //If row is full
            if (GameObject.FindGameObjectsWithTag(y.ToString()).Length == 10)
            {
                //Line count and increase level difficulty
                line_Count++;
                line_Count_Text.text = line_Count.ToString();
                if (line_Count % 10 == 0 && fall_speed > 0.1f)
                {
                    difficulty_level++;
                    level_Text.text = difficulty_level.ToString();
                    fall_speed += -0.1f;

                }
                score_Mul++;
                //Delete the row
                foreach (GameObject gos in GameObject.FindGameObjectsWithTag(y.ToString()))
                {
                    //Need to change the tag or else it's going to find it when updating
                    gos.tag = "-1";
                    Destroy(gos);
                }
                //Update the grid bool array

                for (int x = 1; x < grid_Width - 1; x++)
                {
                    grid[x, y] = false;
                }
                #region Debug messages
                /*
                for (int why = 1; why < 5; why++)
                {
                    string str = "Row " + why + ": ";
                    for (int ex = 1; ex < 11; ex++)
                    {
                        if (grid[ex, why] == true)
                        {
                            str += "t ";
                        }
                        else
                        {
                            str += "f ";
                        }
                    }
                    Debug.Log(str);

                }*/
                #endregion
                //Then move all rows above it down
                MoveAllRowsDown(y + 1);

                #region Debug messages
                //Then move all rows above it down
                /*
                for (int why = 1; why < 5; why++)
                {
                    string str = "Row " + why + ": ";
                    for (int ex = 1; ex < 11; ex++)
                    {
                        if (grid[ex, why] == true)
                        {
                            str += "t ";
                        }
                        else
                        {
                            str += "f ";
                        }
                    }
                    Debug.Log(str);
                    

                }*/
                #endregion
            }

        }
        FindObjectOfType<Score>().updateScore(score_Mul);
        if (score_Mul == 4)
        {
            audio_Source.PlayOneShot(complete_Row4);
        }
        else if (score_Mul >= 1)
        {
            audio_Source.PlayOneShot(complete_Row);
        }
        

    }
    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < grid_Height; i++)
        {
            foreach (GameObject gos in GameObject.FindGameObjectsWithTag(i.ToString()))
            {
                gos.tag = (i - 1).ToString();
                grid[(int)Mathf.Round(gos.transform.position.x), i] = false;
                gos.transform.position += new Vector3(0, -1, 0);
                grid[(int)Mathf.Round(gos.transform.position.x), i-1] = true;
            }
        }
    }

    //Checks if its gameover
    public bool IsGameOver() {


        for (int x = 1; x < grid_Width - 1; x++)
        {
            if (grid[x, 21] == true)
                return true;
        }
        return false;

    }

}
