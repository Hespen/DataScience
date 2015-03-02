using System.Collections.Generic;

public class UserPreference
{
    private readonly Dictionary<int, float> ratings;

    public UserPreference()
    {
        ratings = new Dictionary<int, float>();
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