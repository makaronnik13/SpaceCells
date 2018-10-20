using UnityEngine;

public class Planet
{
    public Sprite sprite;
    public float size;
    public int rating;

    public Planet(Sprite sprite, float size, int rating)
    {
        this.sprite = sprite;
        this.size = size;
        this.rating = rating;
    }
}