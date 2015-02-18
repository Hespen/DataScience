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

    public void addNewRecord(int articleID, float rating)
    {
        if (!ratings.ContainsKey(articleID))
        {
            ratings.Add(articleID, rating);
        }
    }
}
