using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Review
{
    
    private Story review;
    private questionType reviewType;
    private bool done;

    public Review(Story story, questionType type)
    {
        this.review = story;
        this.reviewType = type;
        done = false;
    }

    public Story getReviewContent()
    {
        return review;
    }

    public bool getDone()
    {
        return done;
    }
    
    public questionType getReviewType()
    {
        return reviewType;
    }

    public void doneShowReview()
    {
        done = true;
    }

    public void resetReview()
    {
        done = false;
    }
}
