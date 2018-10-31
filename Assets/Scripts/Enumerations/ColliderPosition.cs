using UnityEngine;

public enum ColliderPosition {
    FRONTTOP,
    FRONTUP,
    FRONTMIDDLE,
    FRONTDOWN,
    BACKTOP,
    BACKUP,
    BACKMIDDLE,
    BACKDOWN,
    BOTTOM
}

public static class EnumColliderPosition { 

    public static int ToInt(ColliderPosition colliderPosition) {
        switch (colliderPosition) {
            case ColliderPosition.FRONTTOP:
                return 0;
                //break;
            case ColliderPosition.FRONTUP:
                return 1;
                //break;
            case ColliderPosition.FRONTMIDDLE:
                return 2;
                //break;
            case ColliderPosition.FRONTDOWN:
                return 3;
                //break;
            case ColliderPosition.BACKTOP:
                return 4;
                //break;
            case ColliderPosition.BACKUP:
                return 5;
                //break;
            case ColliderPosition.BACKMIDDLE:
                return 6;
                //break;
            case ColliderPosition.BACKDOWN:
                return 7;
                //break;
            case ColliderPosition.BOTTOM:
                return 8;
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
                return ColliderPosition.FRONTUP;
                //break;
            case 2:
                return ColliderPosition.FRONTMIDDLE;
                //break;
            case 3:
                return ColliderPosition.FRONTDOWN;
                //break;
            case 4:
                return ColliderPosition.BACKTOP;
                //break;
            case 5:
                return ColliderPosition.BACKUP;
                //break;
            case 6:
                return ColliderPosition.BACKMIDDLE;
                //break;
            case 7:
                return ColliderPosition.BACKDOWN;
                //break;
            case 8:
                return ColliderPosition.BOTTOM;
                //break;
            default:
                Debug.Log("Wrong value of intColliderPosition into ColliderPosition.ToColliderPosition function!");
                return ColliderPosition.FRONTTOP;
        }
    }
}
