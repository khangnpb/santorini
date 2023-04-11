using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Each Player has at least one male and one female worker that they can move around the board and build around. Some gods allow you to have more workers
/// </summary>
public class Worker : MonoBehaviour
{
    public enum Gender
    {
        Female,
        Male
    }

    public enum Colour
    {
        Blue, 
        White,
        Red,
        Green
    }
    
    [SerializeField]
    GameObject _highlight = default;

    Player _player = default;
    Tile _tile = default;

    Gender _gender = Gender.Female;
    Colour _colour = Colour.Blue;

    public void Initialize(Gender gender, Colour colour)
    {
        _gender = gender;
        _colour = colour;
    }

    public void EnableHighlight()
    {
        _highlight.SetActive(true);
    }

    public void DisableHighlight()
    {
        _highlight.SetActive(false);
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public Player GetPlayer()
    {
        return _player;
    }

    public void SetTile(Tile tile)
    {
        _tile = tile;
    }

    public Tile GetTile()
    {
        return _tile;
    }

    public Gender GetGender()
    {
        return _gender;
    }

    public Colour GetColour()
    {
        return _colour;
    }
}
