using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    //Used to for making the block fall
    private float fall = 0;
    //Move speed
    private float move = 0;
    private float move_speed = 0.1f;
    //Down speed (reason why i don't use move speed is because it won't let you move both at same time)
    private float down = 0;
    private float down_speed = 0.1f;
    //Only allow players to hold once for every block
    private bool hold_Once = false;
    


    //Limit rotations for long, s and z. Don't allow rotations for square
    public bool limit_Rotations = false;
    public bool allow_Rotations = true;

    //Sound components
    public AudioClip rotate_Sound;
    public AudioClip land_Sound;
    public AudioClip hold_Sound;
    private AudioSource audio_Source;



    void Start()
    {
        Time.timeScale = 1f;
        PauseMenu.Game_Is_Paused = false;

        audio_Source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (FindObjectOfType<Game>().IsGameOver())
        {
            FindObjectOfType<Game>().mPanelGameover.GetChild(0).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Grid").GetComponent<AudioSource>().enabled = false;
        }
        else if (PauseMenu.Game_Is_Paused)
        {

        }
        else
        {
            checkInput();
        }
        
    }
    
    //Method for checking input
    void checkInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Everything can rotate except squares
            if (allow_Rotations)
            {
                audio_Source.PlayOneShot(rotate_Sound);
                //long, s and z gets limited rotations
                if (limit_Rotations)
                {
                    if (transform.rotation.eulerAngles.z == 90)
                    {
                        transform.Rotate(0, 0, -90);
                        if (!ValidPosition())
                        {
                            this.transform.Rotate(0, 0, 90);
                        }
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                        if (!ValidPosition())
                        {
                            this.transform.Rotate(0, 0, -90);
                        }
                    }
                }
                else
                {
                    this.transform.Rotate(0, 0, 90);
                    if (!ValidPosition())
                    {
                        this.transform.Rotate(0, 0, -90);
                    }
                }
            }
        }
        if ((Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= FindObjectOfType<Game>().fall_speed) && Time.time - down >= down_speed)
        {
            this.transform.position += new Vector3(0, -1, 0);
            //When you hit the ground, you have to spawn a new block and disable this one and update the grid too
            if (!ValidPosition())
            {
                audio_Source.PlayOneShot(land_Sound);
                this.transform.position += new Vector3(0, 1, 0);
                enabled = false;
                //FindObjectOfType<Game>().SpawnRandomTetromino();
                FindObjectOfType<Game>().SpawnNextTetro();
                FindObjectOfType<Game>().UpdateGrid(this);
                FindObjectOfType<Game>().deleteRow(this);
                hold_Once = false;
                /*
                if (FindObjectOfType<Game>().IsGameOver()){
                    FindObjectOfType<Game>().GameOver();
                }*/
            }
            fall = Time.time;
            down = Time.time;
        }
        //Fast drop! That was easy.
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (ValidPosition())
            {
                this.transform.position += new Vector3(0, -1, 0);

            }
            if (!ValidPosition())
            {
                audio_Source.PlayOneShot(land_Sound);
                this.transform.position += new Vector3(0, 1, 0);
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetro();
                FindObjectOfType<Game>().UpdateGrid(this);
                FindObjectOfType<Game>().deleteRow(this); //THIS WILL ALSO UPDATE SCORE
                hold_Once = false;

            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Time.time - move >= move_speed)
        {
            this.transform.position += new Vector3(-1, 0, 0);
            //If this is not a valid position then move it back
            if (!ValidPosition())
            {
                this.transform.position += new Vector3(1, 0, 0);
            }
            move = Time.time;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Time.time - move >= move_speed)
        {
            this.transform.position += new Vector3(1, 0, 0);
            if (!ValidPosition())
            {
                this.transform.position += new Vector3(-1, 0, 0);
            }
            move = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!hold_Once)
            {
                FindObjectOfType<Game>().HoldTetro();
                audio_Source.PlayOneShot(hold_Sound);
                hold_Once = true;
            }

        }
    }

    //Will check if it goes pass the grid. Used in check input
    public bool ValidPosition()
    {
        //Iterate each block of the tetromino and check if its not passed the walls or passed the ground
        foreach (Transform mino in transform)
        {
            #region old verision
            /*
            if (left_wall.position.x >= Mathf.Round(mino.position.x)
                || right_wall.position.x <= Mathf.Round(mino.position.x) 
                || ground.position.y >= Mathf.Round(mino.position.y))
            {
                return false;
            }*/
            #endregion
            if (!FindObjectOfType<Game>().inGrid(mino))
            {
                return false;
            }
        }
        return true;
    }

    

}
