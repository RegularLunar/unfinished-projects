namespace CS2
{
    public static class Offsets
    {
        //buttons.cs
        public static int jump = 0x182EBC0; // this can be found in buttons.cs
        public static int dwForceAttack = 0x182E6B0;// this can be found in buttons.cs - Attack1

        //offsets.cs
        public static int dwViewMatrix = 0x1A32DB0;
        public static int dwLocalPlayerPawn = 0x1835BB8;
        public static int dwLocalPlayerController = 0x1A20970;
        public static int dwEntityList = 0x19D0A38;
        public static int dwViewAngles = 0x1A3CC40;
        public static int dwGameRules = 0x1A2E6E8;
        public static int dwWeaponC4 = 0x19D3E20;

        //client.cs
        public static int m_iFOV = 0x210;
        public static int m_pCameraServices = 0x11E0;
        public static int m_bIsScoped = 0x23D0;
        public static int m_hObserverTarget = 0x44;
        public static int m_pObserverServices = 0x11C0;
        public static int m_hPlayerPawn = 0x80C;
        public static int m_flFlashBangTime = 0x13F8;
        public static int m_iIDEntIndex = 0x1458;
        public static int m_iTeamNum = 0x3E3;
        public static int m_bSpotted = 0x8;
        public static int m_iszPlayerName = 0x660;
        public static int m_entitySpottedState = 0xFA0;
        public static int m_iHealth = 0x344;
        public static int m_vOldOrigin = 0x1324;
        public static int m_vecViewOffset = 0xCB0;
        public static int m_lifeState = 0x348;
        public static int m_modelState = 0x170;
        public static int m_pGameSceneNode = 0x328;
        public static int m_vecAbsVelocity = 0x3F0;
        public static int m_fFlags = 0x3EC;
        public static int m_bBombPlanted = 0x1B7B;
        public static int m_pClippingWeapon = 0x13A0;
        public static int m_Item = 0x50;
        public static int m_AttributeManager = 0x1240;
        public static int m_iItemDefinitionIndex = 0x1BA;
        public static int m_pInGameMoneyServices = 0x720;
        public static int m_iAccount = 0x40;
        public static int m_iTotalCashSpent = 0x48;
        public static int m_iCashSpentThisRound = 0x4C;
        public static int m_aimPunchAngle = 0x1584;
        public static int m_iShotsFired = 0x23E4;
        public static int m_flNextGlow = 0xFB8;
        public static int m_hController = 0x133C;
        public static int m_iObserverMode = 0x40;
        public static int m_sSanitizedPlayerName = 0x770;
    }
}
