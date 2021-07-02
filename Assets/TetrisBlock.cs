using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour {

    private float previousTime;
    public float fallTime = 0.8f;

    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];

    public Vector3 rotationPoint;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            transform.position += new Vector3(-1, 0, 0);
            if(!canMove()) {
                transform.position -= new Vector3(-1, 0, 0);
            }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow)) {
            transform.position += new Vector3(1, 0, 0);
            if (!canMove()) {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if(!canMove()) {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime /10 : fallTime )) {
            transform.position += new Vector3(0, -1, 0);
            if (!canMove()) {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckLine();
                this.enabled = false;
                FindObjectOfType<TetrominoSpawner>().NewTetromino();
            }
            previousTime = Time.time;
        }


    }

    private void AddToGrid() {
        foreach (Transform children in transform) {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);            
            grid[roundedX, roundedY] = children;
            print(grid[roundedX, roundedY]);
        }
    }

    private void CheckLine() {
        for(int i = height - 1; i >= 0; i--) {
            if(FullLine(i)) {
                ClearLine(i);
                RowDown(i);
            }
        }
    }

    private bool FullLine(int i) {
        for(int j = 0; j < width; j++) {
            if(grid[j, i] == null) {
                return false;
            }
        }
        return true;
    }

    private void ClearLine(int i) {
        for (int j = 0; j < width; j++) {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    private void RowDown(int i) {
        for(int y = i; y < height; y++) {
            for(int j = 0; j < width; j++) {
                if(grid[j, y] != null) {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private bool canMove() {
        foreach(Transform children in transform) {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height) {
                return false;
            }
            if(grid[roundedX, roundedY] != null) {
                print("YES");
                return false;
            }
        }

        return true;
    }
}
