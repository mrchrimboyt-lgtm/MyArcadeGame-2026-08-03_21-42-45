using UnityEngine;

public class PlayerSprites : MonoBehaviour
{
    public SpriteRenderer PlayerspriteRenderer;//player sprite
    public SpriteRenderer ClothspriteRenderer;//the player colour sprite
    public GameObject Balloon;//ballon sprite
    public SpriteRenderer BalloonspriteRenderer;
    //animation infomation
    public SpriteAnimationSet[] AnimationSets; //0walking 1fall 2jump 3grabside, 4grabmiddle, 5fullfall, 6balloon
    
    public void UpdateSprite(int State, int Xdir, int Ydir, int facingblock) //Updates sprite to sprite set based on these player varibles
    {
        if (facingblock == -2 && State == 1) { return; }//if facingblock is -2 and in grab mode the code will ingore the update. This is used to ignore movement sprite updates
        PlayerspriteRenderer.flipX = Xdir < 0;//set sprite directions to the Xdir
        ClothspriteRenderer.flipX = Xdir < 0;
        BalloonspriteRenderer.flipX = Xdir < 0;
        switch (State) { //rules for each player state
            case 0: //normal
                int[] yindexvalues = new int[3] {1,0,2};//if Ydir is -1 use animation set 1. if Ydir is -1 use animation set 0 if Ydir is 1 use animation set 2
                LoadAnimationSet(yindexvalues[Ydir+1]);
                break;
            case 1: //grab
                if (facingblock == 0) { LoadAnimationSet(4); }//if in the middle of a block
                else { LoadAnimationSet(3); }
                PlayerspriteRenderer.flipX = facingblock < 0; //if on the side of a block face sprite to the block
                ClothspriteRenderer.flipX = facingblock < 0;
                break;
            case 2: //fall
                LoadAnimationSet(5);
                break;
            case 3://balloon
                LoadAnimationSet(6);
                break;
        }
    }

    private void LoadAnimationSet(int index)//loads sprite based on animation
    {
        PlayerspriteRenderer.sprite= AnimationSets[index].playersprites[AnimationSets[index].currentframe];//Updates all the sprites
        ClothspriteRenderer.sprite = AnimationSets[index].clothsprites[AnimationSets[index].currentframe];
        Balloon.SetActive(AnimationSets[index].Balloon);//turns on or off the balloon object
        AnimationSets[index].currentframe++; //increases the current frame. (This is for animation such as falling, most animations are one frame)
        if(AnimationSets[index].currentframe >= AnimationSets[index].playersprites.Count) {
            AnimationSets[index].currentframe=0;
        }
    }
}
