using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialPage : MonoBehaviour
{
    public TextMeshProUGUI gameName, t1, t2, t3, t4, t5;
    public Image[] imgAra = new Image[4];
    int game_number = 0;
    string[] spriteName = { "tic_tac_toe1", "Tic_Tac_Toe2", "Tic_Tac_Toe6", "Tic_Tac_Toe8" };
    string s1 = "Tic Tac Toe";
    string s2 = "1. This can be played in three different grid size: Grid 1: 3*3 size, Grid 2: 4*4 size and Grid 3: 6*7 size.\n2. just touch on the grid you want to make your move and the corresponding symbol will appear.";
    string s3 = "3. For Grid 1, you just have to match 3 consecutive symbols- either row wise or column wise or diagonally. Also block opponents from doing so.";
    string s4 = "4. For Grid 2, it is a 4*4 grid. You just have to match 4 consecutive symbols either row wise or column wise or diagonally. Here all rules are same as 3*3 grid.";
    string s5 = "5. For Grid 3, there is 1 point for each 3 consecutive same symbol. Unlike all previous modes, here game will finish only after all cell get filled up and the player with the highest point will win.";
    string s6 = "Game is Available in mainly two mode: Single Player and Two Player.\n(i). Single Player: One Player will play against the computer player.\nThree different difficulty level for computer player:\n\t(i). Easy\n\t(ii). Medium\n\t(iii). Hard\n\n(ii). Two Player: Two Human player will play on the same device.";

    void Start()
    {
        game_number = PlayerPrefs.GetInt("valueGame", 0);
        if (game_number == 0)
        {
            //tic
            spriteName[0] = "tic_tac_toe1";
            spriteName[1] = "Tic_Tac_Toe2";
            spriteName[2] = "Tic_Tac_Toe6";
            spriteName[3] = "Tic_Tac_Toe8";
            s1 = "Tic Tac Toe";
            s2 = "1. This can be played in three different grid size: small: 3*3 size, medium: 4*4 size and large: 6*7 size.\n2. just touch on the grid you want to make your move and the corresponding symbol will appear.";
            s3 = "3. For Small grid you just have to match 3 consecutive symbols- either row wise or column wise or diagonally. Also block opponents from doing so.";
            s4 = "4. For Medium grid, it is a 4*4 grid. You just have to match 4 consecutive symbols either row wise or column wise or diagonally. Here all rules are same as the small grid.";
            s5 = "5. For Large grid, there is 1 point for each 3 consecutive same symbol. Unlike all previous modes, here game will finish only after all cell get filled up and the player with the highest point will win.";
            s6 = "6. Some additional rules to keep in mind:\n(i). Players can only place one mark at a time.\n(ii). Players cannot place a mark in a cell that has already been filled.\n(iii). Players cannot move or remove their marks once they have been placed.";

        }
        else if (game_number == 1)
        {
            //dots
            spriteName[0] = "Dot_and_Boxes1";
            spriteName[1] = "Dot_and_Boxes2";
            spriteName[2] = "Dot_and_Boxes3";
            spriteName[3] = "Dot_and_Boxes4";
            s1 = "Dots and Boxes";
            s2 = "1. Dots and Boxes is a two-player game played on a grid of dots. The goal of the game is to have more boxes than your opponent at the end of the game.";
            s3 = "2. Here Small grid size is 6x5, Medium grid size is 8x5 and Large grid size is 10x5. The first player will be chosen randomly and draws a line connecting two adjacent dots.Then the second player takes a turn and draws a line connecting two adjacent dots. The main target is to create a box and get a point.";
            s4 = "3. When a player completes a box, they get 1 point and gets another turn. The game will continue until all of the boxes are completed. The player with the most completed boxes at the end of the game wins.";
            s5 = "4. Some additional rules to keep in mind:\n(i). Players can only draw one line at a time.\n(ii). Lines can only be drawn between two adjacent dots.\n(iii). Players cannot draw a line that completes a box on their opponent's turn.\n(iv). When a player completes a box, they get another turn.\n(v).";
        }
        else if (game_number == 2)
        {
            //sim
            spriteName[0] = "SIM1";
            spriteName[1] = "SIM2";
            spriteName[2] = "SIM3";
            spriteName[3] = "SIM4";
            s1 = "SIM GAME";
            s2 = "1. Sim is a two-player game played on a sheet of paper with a grid of dots. The goal of the game is to be the last player to make a move and create a triangle.The will be the winner immediately and the game will end.";
            s3 = "2. The first player draws a line connecting any two adjacent dots. The second player then takes a turn and draws a line connecting any two adjacent dots that are not already connected. Players take turns drawing lines until a player creates a triangle with lines that connect three dots.";
            s4 = "3. There are 3 maps to play. Player can select any map to play in 3 different modes.";
            s5 = "4. Some additional rules to keep in mind:\n(i). Lines can only be drawn between two adjacent dots.\n(ii). Players cannot draw a line that completes a triangle on their opponent's turn.\n(iii). When a player creates a triangle, they get another turn.\n(iv). If a player creates a triangle that has already been created by their opponent, they do not get another turn.";
        }
        else if (game_number == 3)
        {
            //sos
            spriteName[0] = "SOS1";
            spriteName[1] = "SOS2";
            spriteName[2] = "SOS3";
            spriteName[3] = "SOS4";
            s1 = "SOS GAME";
            s2 = "1. SOS is a two-player game played on a grid of 6x7. The goal of the game is to create as many 'SOS' sequences as possible.";
            s3 = "2. An \"SOS\" sequence is created when a player forms a line of three consecutive letters 'S', 'O', and 'S' in a row, column, or diagonal of the grid. But completing \"SOS\" using already created \"SOS\" will not bring you any point.";
            s4 = "3. The player who goes first can choose to place an \"S\" or an \"O\" in any empty cell of the grid. The second player then takes a turn and places their letter in an empty cell. Players take turns placing their letters until the grid is filled. The player who created the most \"SOS\" sequences at the end of the game wins.";
            s5 = "4. Some additional rules to keep in mind:\n(i). A player can only place one letter at a time.\n(ii). A player cannot place a letter in a cell that has already been filled.\n(iii). A player cannot change the letter they have already placed in a cell.\n(iv). If a player creates an \"SOS\" sequence, they get to take another turn.\n(v). If the grid is filled and there are no \"SOS\" sequences, the game ends in a tie.";
        }
        else if (game_number == 4)
        {
            //four
            spriteName[0] = "4_in_a_row2";
            spriteName[1] = "4_in_a_row3";
            spriteName[2] = "4_in_a_row4";
            spriteName[3] = "4_in_a_row5";
            s1 = "Four in a Row";
            s2 = "1. Four in a Row has 3 board- Small, Medium and Large. The goal of the game is to be the first player to connect your own 4 consecutive pieces vertically, horizontally, or diagonally.";
            s3 = "2. The player who goes first will use the \"Blue\" pieces and the second player will use the \"Red\" pieces. The first player chooses a column and drops their piece into the lowest available space in that column. The second player then takes a turn and drops their piece into a column of their choice.";
            s4 = "3. Players take turns dropping their pieces until one player connects four of their own pieces vertically, horizontally, or diagonally, or until the board is filled. If a player connects four of their pieces, they win the game. If the board is filled and there are no four-in-a-row connections, the game ends in a tie.";
            s5 = "4. Some additional rules to keep in mind:\n(i). Players can only drop one piece at a time.\n(ii). Players cannot drop a piece into a column that is already full.\n(iii). The pieces must be dropped into the lowest available space in the chosen column.\n(iv). Players cannot move pieces once they have been dropped into a column.\n(v) All the available valid move is marked for a player to play";
        }
        else
        {
            //twoguti
            spriteName[0] = "Pong_Hu1";
            spriteName[1] = "Pong_Hu2";
            spriteName[2] = "Pong_Hu3";
            spriteName[3] = "SIM_img4";
            s1 = "Pong Hau";
            s2 = "1. Pong Hau K'i is a two-player game that is traditionally played on a board with a grid of four dots connected by lines. The main target is to block opponents movement.";
            s3 = "2. The first player uses the two dots on one side of the board, and the second player uses the two dots on the opposite side. The first player moves one of their pieces to an adjacent empty space. The second player then takes a turn and moves one of their pieces to an adjacent empty space.";
            s4 = "3. Players take turns moving their pieces until one player cannot make a move. If a player cannot make a move, loses the game and the oponent wins.";
            s5 = "4. Some additional rules to keep in mind:\n(i). Players can only move one piece at a time.\n(ii). Pieces cannot move to a space that is already occupied by another piece.\n(iii). Pieces can only move to an adjacent empty space that is directly connected by a line.";
        }


        gameName.text = s1;
        t1.text = s2;
        t2.text = s3;
        t3.text = s4;
        t4.text = s5;
        t5.text = s6;


        for (int i = 0; i < 4; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(spriteName[i]);
            imgAra[i].sprite = sprite;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("parentpage");
        }
    }
}
