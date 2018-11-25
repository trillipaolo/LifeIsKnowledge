using UnityEngine;
using System;

public enum ColliderPosition {
    FRONTTOP,
    FRONTMIDDLE,
    FRONTDOWN,
    BACKTOP,
    BACKMIDDLE,
    BACKDOWN
}

public static class EnumColliderPosition {

    public static int Size() {
        return Enum.GetValues(typeof(ColliderPosition)).Length;
    }

    //OPTIMIZATION: we can create a variable size and a method that at the beginning sets the right value instead of computing it every time
    public static bool[] StringToArray(string colliderString) {
        if(colliderString == null) {
            Debug.Log("colliderString into ColliderPosition.StringToArray function is null!");
            return null;
        }
        int colliderPositionSize = Size();
        if (colliderString.Length != colliderPositionSize) {
            Debug.Log("colliderString into ColliderPosition.StringToArray function has a different length from the size of ColliderPosition enumeration!");
        }

        bool[] colliderArray = new bool[colliderPositionSize];
        for(int i = 0; i < colliderPositionSize; i++) {
            colliderArray[i] = colliderString.Substring(i,1) == "1";
        }
 
        return colliderArray;
    }

    public static int ToInt(ColliderPosition colliderPosition) {
        switch (colliderPosition) {
            case ColliderPosition.FRONTTOP:
                return 0;
                //break;
            case ColliderPosition.FRONTMIDDLE:
                return 1;
                //break;
            case ColliderPosition.FRONTDOWN:
                return 2;
                //break;
            case ColliderPosition.BACKTOP:
                return 3;
                //break;
            case ColliderPosition.BACKMIDDLE:
                return 4;
                //break;
            case ColliderPosition.BACKDOWN:
                return 5;
                //break;
            default:
                Debug.Log("Wrong value of colliderPosition into ColliderPosition.ToInt function!");
                return 0;
        }
    }

    public static ColliderPosition ToColliderPosition(int intColliderPosition) {
        switch (intColliderPosition) {
            case 0:
                return ColliderPosition.FRONTTOP;
                //break;
            case 1:
                return ColliderPosition.FRONTMIDDLE;
                //break;
            case 2:
                return ColliderPosition.FRONTDOWN;
                //break;
            case 3:
                return ColliderPosition.BACKTOP;
                //break;
            case 4:
                return ColliderPosition.BACKMIDDLE;
                //break;
            case 5:
                return ColliderPosition.BACKDOWN;
                //break;
            default:
                Debug.Log("Wrong value of intColliderPosition into ColliderPosition.ToColliderPosition function!");
                return ColliderPosition.FRONTTOP;
        }
    }


}
