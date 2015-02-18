using System;
using System.Collections;
using System.Collections.Generic;

public class UserPreference
{
    private Dictionary<int,float> ratings;
    public UserPreference()
    {
        this.ratings = new Dictionary<int, float>();
    }

    public void AddNewRecord(int articleId, float rating)
    {
        if (!ratings.ContainsKey(articleId))
        {
            ratings.Add(articleId, rating);
        }
    }

    public Dictionary<int, float> GetRatings()
    {
        return ratings;
    }

    public float GetRating(int articleId)
    {
        return ratings.ContainsKey(articleId) ? ratings[articleId] : 0;
    }
}
