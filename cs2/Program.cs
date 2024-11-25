using Swed64;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Timers;

namespace CS2
{
    internal static class Program
    {
        const int INAIR = 65664;
        const int PLUS = 65537;
        const int MINUS = 256;
        const uint STANDING = 65665;
        const uint CROUCHING = 65667;
        [DllImport("User32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        static System.Timers.Timer timer = new System.Timers.Timer();

        [STAThread]
        private static void Main()
        {
            Swed swed = new Swed("cs2");
            IntPtr client = swed.GetModuleBase("client.dll");
            Vector3 velocity;
            Renderer renderer = new Renderer();
            Thread renderThread = new Thread(() => renderer.Start().Wait());
            renderThread.Start();
            renderThread.Join();
            Vector2 screenSize = renderer.screenSize;
            List<Entity> entities = new List<Entity>();
            List<Entity> _entities = new List<Entity>();
            List<Entity> nentities = new List<Entity>();
            List<Entity> espentities = new List<Entity>();
            List<string> spectators = new List<string>();
            Entity EspLocalPlayer = new Entity();
            Entity localPlayer = new Entity();
            IntPtr forceJumpAddress = client + Offsets.jump;
            while (renderer.isRunning)
            {
                if (renderer.LessCpuUsageButLag)
                {
                    Thread.Sleep(renderer.delay);
                }

                IntPtr localPlayerPawn = swed.ReadPointer(client, Offsets.dwLocalPlayerPawn);
                IntPtr entityList = swed.ReadPointer(client, Offsets.dwEntityList);

                if (renderer.FovEnabled)
                {
                    uint desiredFov = (uint)renderer.fov;
                    IntPtr cameraServices = swed.ReadPointer(localPlayerPawn, Offsets.m_pCameraServices);
                    uint CurrentFov = swed.ReadUInt(cameraServices + Offsets.m_iFOV);
                    bool isScoped = swed.ReadBool(localPlayerPawn, Offsets.m_bIsScoped);
                    if (!isScoped && CurrentFov != desiredFov)
                    {
                        swed.WriteUInt(cameraServices + Offsets.m_iFOV, desiredFov);
                    }
                }
                else if (renderer.AutoGetFovWhileDisabled)
                {
                    uint desiredFov = (uint)renderer.fov;
                    IntPtr cameraServices = swed.ReadPointer(localPlayerPawn, Offsets.m_pCameraServices);
                    uint CurrentFov = swed.ReadUInt(cameraServices + Offsets.m_iFOV);
                    bool isScoped = swed.ReadBool(localPlayerPawn, Offsets.m_bIsScoped);
                    if (!isScoped && CurrentFov != desiredFov)
                    {
                        renderer.fov = (int)swed.ReadUInt(cameraServices + Offsets.m_iFOV);
                    }
                }
                if (renderer.NoFlash)
                {

                    float flashDuration = swed.ReadFloat(localPlayerPawn, Offsets.m_flFlashBangTime);
                    if (flashDuration > 0)
                    {
                        swed.WriteFloat(localPlayerPawn, Offsets.m_flFlashBangTime, 0);
                    }
                }
                if (renderer.Triggerbot)
                {

                    int Team = swed.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);
                    int entIndex = swed.ReadInt(localPlayerPawn, Offsets.m_iIDEntIndex);

                    if (entIndex != -1)
                    {
                        IntPtr listEntry = swed.ReadPointer(entityList, 0x8 * ((entIndex & 0x7FFF) >> 9) + 0x10);
                        IntPtr CurrentPawn = swed.ReadPointer(listEntry, 0x78 * (entIndex & 0x1FF));
                        int entityTeam = swed.ReadInt(CurrentPawn, Offsets.m_iTeamNum);

                        if (entityTeam != Team)
                        {
                            if (GetAsyncKeyState(renderer.TBHotKey) < 0)
                            {
                                float flashDuration = swed.ReadFloat(localPlayerPawn, Offsets.m_flFlashBangTime);
                                if (renderer.FlashCheckTrigger && flashDuration > 0 && flashDuration <= 0.1f)
                                {
                                    return;
                                }
                                swed.WriteInt(client + Offsets.dwForceAttack, 65537);
                                Thread.Sleep(10);
                                swed.WriteInt(client + Offsets.dwForceAttack, 256);
                                Thread.Sleep(10);
                            }
                        }
                    }
                }
                if (renderer.Radar)
                {
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);
                    for (int i = 0; i < 64; i++)
                    {
                        if (listEntry == IntPtr.Zero)
                            continue;

                        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

                        if (currentController == IntPtr.Zero)
                            continue;

                        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);
                        if (pawnHandle == 0)
                            continue;
                        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);

                        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

                        string name = swed.ReadString(currentController, Offsets.m_iszPlayerName);
                        bool spotted = swed.ReadBool(currentPawn, Offsets.m_entitySpottedState + Offsets.m_bSpotted);

                        swed.WriteBool(currentPawn, Offsets.m_entitySpottedState + Offsets.m_bSpotted, true);


                    }
                }
                if (renderer.AimbotEnabled)
                {
                    entities.Clear();
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);
                    localPlayer.pawnAddress = localPlayerPawn;
                    localPlayer.team = swed.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);
                    localPlayer.origin = swed.ReadVec(localPlayerPawn, Offsets.m_vOldOrigin);
                    localPlayer.view = swed.ReadVec(localPlayerPawn, Offsets.m_vecViewOffset);
                    entities.Clear();
                    for (int i = 0; i < 64; i++)
                    {
                        if (listEntry == IntPtr.Zero)
                            continue;

                        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

                        if (currentController == IntPtr.Zero)
                            continue;

                        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);
                        if (pawnHandle == 0)
                            continue;

                        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                        if (listEntry2 == IntPtr.Zero)
                            continue;

                        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));
                        if (currentPawn == IntPtr.Zero)
                            continue;

                        IntPtr sceneMode = swed.ReadPointer(currentPawn, Offsets.m_pGameSceneNode);

                        IntPtr boneMatrix = swed.ReadPointer(sceneMode, Offsets.m_modelState + 0x80);

                        int health = swed.ReadInt(currentPawn, Offsets.m_iHealth);
                        int team = swed.ReadInt(currentPawn, Offsets.m_iTeamNum);
                        uint lifeState = swed.ReadUInt(currentPawn, Offsets.m_lifeState);
                        bool spotted = swed.ReadBool(currentPawn, Offsets.m_entitySpottedState + Offsets.m_bSpotted);

                        if (spotted == false && renderer.aimOnlyatSpotted)
                            continue;

                        if (lifeState != 256) continue;
                        if (team == localPlayer.team && !renderer.AimOnTeam) continue;

                        Entity entity = new Entity();
                        entity.pawnAddress = currentPawn;
                        entity.controllerAddress = currentController;
                        entity.health = health;
                        entity.team = team;
                        entity.lifeState = lifeState;
                        entity.origin = swed.ReadVec(currentPawn, Offsets.m_vOldOrigin);
                        entity.view = swed.ReadVec(currentPawn, Offsets.m_vecViewOffset);
                        entity.distance = Vector3.Distance(entity.origin, localPlayer.origin);
                        entity.head = swed.ReadVec(boneMatrix, 6 * 32);
                        ViewMatrix matrix = ReadMatrix(client + Offsets.dwViewMatrix);
                        entity.head2d = Calculate.WorldToScreen(matrix, entity.head, (int)screenSize.X, (int)screenSize.Y);
                        entity.PixelDistance = Vector2.Distance(entity.head2d, new Vector2(screenSize.X / 2, screenSize.Y / 2));
                        entities.Add(entity);

                    }
                    entities = entities.OrderBy(o => o.PixelDistance).ToList();
                    if (entities.Count > 0 && GetAsyncKeyState(renderer.AbHotKey) < 0)
                    {
                        Vector3 playerView = Vector3.Add(localPlayer.origin, localPlayer.view);
                        Vector3 entityView = Vector3.Add(entities[0].origin, entities[0].view);
                        if (entities[0].PixelDistance < renderer.AimbotFOV)
                        {
                            float flashDuration = swed.ReadFloat(localPlayerPawn, Offsets.m_flFlashBangTime);
                            if (renderer.FlashCheckAim && flashDuration > 0 && flashDuration <= 0.1f)
                            {
                                return;
                            }
                            if (renderer.LessCpuUsageButLag)
                            {
                                Vector2 newAngles = newAngles = Calculate.CalculateAngles(playerView, entities[0].head);
                                Vector3 currentAngles = swed.ReadVec(client, Offsets.dwViewAngles);
                                Vector3 targetAngles = new Vector3(newAngles.Y, newAngles.X, 0.0f);
                                Vector3 smoothAngles = Vector3.Lerp(currentAngles, targetAngles, (renderer.smoothness / 100f));

                                swed.WriteVec(client, Offsets.dwViewAngles, smoothAngles);
                            }
                            else
                            {
                                Vector2 newAngles = newAngles = Calculate.CalculateAngles(playerView, entities[0].head);
                                Vector3 currentAngles = swed.ReadVec(client, Offsets.dwViewAngles);
                                Vector3 targetAngles = new Vector3(newAngles.Y, newAngles.X, 0.0f);
                                Vector3 smoothAngles = Vector3.Lerp(currentAngles, targetAngles, (renderer.smoothness / 1000f));

                                swed.WriteVec(client, Offsets.dwViewAngles, smoothAngles);
                            }
                        }
                    }
                }
                if (renderer.shootOnjumpShot)
                {
                    int fFlag = swed.ReadInt(localPlayerPawn, Offsets.m_fFlags);
                    if (fFlag == INAIR && GetAsyncKeyState(renderer.AJHotKey) < 0)
                    {
                        Thread.Sleep(100);
                        velocity = swed.ReadVec(localPlayerPawn, Offsets.m_vecAbsVelocity);
                        while (velocity.Z > 18f || velocity.Z < -18f)
                        {
                            velocity = swed.ReadVec(localPlayerPawn, Offsets.m_vecAbsVelocity);
                        }
                        swed.WriteInt(client, Offsets.dwForceAttack, PLUS);
                        Thread.Sleep(150);
                        swed.WriteInt(client, Offsets.dwForceAttack, MINUS);
                        Thread.Sleep(1000);
                    }
                }
                if (renderer.AutoBHOP)
                {
                    uint fFlag = swed.ReadUInt(localPlayerPawn, Offsets.m_fFlags);
                    if (GetAsyncKeyState(renderer.BHHotKey) < 0)
                    {
                        if (fFlag == STANDING || fFlag == CROUCHING)
                        {
                            Thread.Sleep(renderer.bob);
                            swed.WriteUInt(forceJumpAddress, PLUS);
                        }
                        else
                        {
                            swed.WriteUInt(forceJumpAddress, MINUS);
                            Thread.Sleep(renderer.bob / 2);
                        }
                    }
                }
                if (renderer.WeaponESP)
                {
                    _entities.Clear();
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);
                    for (int i = 0; i < 64; i++)
                    {
                        if (listEntry == IntPtr.Zero)
                            continue;
                        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);
                        if (currentController == IntPtr.Zero)
                            continue;
                        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);
                        if (pawnHandle == 0)
                            continue;
                        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));
                        if (currentPawn == IntPtr.Zero)
                            continue;
                        int team = swed.ReadInt(currentPawn, Offsets.m_iTeamNum);
                        int currentTeam = swed.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);

                        IntPtr currentWeapon = swed.ReadPointer(currentPawn, Offsets.m_pClippingWeapon);
                        short weaponDefIndex = swed.ReadShort(currentWeapon, Offsets.m_AttributeManager + Offsets.m_Item + Offsets.m_iItemDefinitionIndex);
                        if (weaponDefIndex == -1) continue;

                        ViewMatrix vw = ReadMatrix(client + Offsets.dwViewMatrix);

                        Entity entity = new Entity();
                        entity.currentWeaponIndex = weaponDefIndex;
                        entity.position = swed.ReadVec(currentPawn, Offsets.m_vOldOrigin);
                        entity.position2D = Calculate.WorldToScreen(vw, entity.position, (int)screenSize.X, (int)screenSize.Y);
                        entity.team = team;
                        entity.PlayerTeamHolder = currentTeam;
                        entity.currentWeaponName = Enum.GetName(typeof(Weapon), weaponDefIndex);
                        _entities.Add(entity);
                        renderer.entities = _entities;
                    }
                }
                if (renderer.EnableStatus)
                {
                    Vector3 vel = swed.ReadVec(localPlayerPawn, Offsets.m_vecAbsVelocity);
                    int speed = (int)Math.Sqrt(vel.X * vel.X + vel.Y * vel.Y + vel.Z * vel.Z);
                    renderer.speed = speed;
                    var gameRules = swed.ReadPointer(client, Offsets.dwGameRules);
                    if (gameRules != IntPtr.Zero)
                    {
                        renderer.bombPlanted = swed.ReadBool(gameRules, Offsets.m_bBombPlanted);
                        if (renderer.bombPlanted)
                        {
                            timer.Interval = 1000;
                            timer.Enabled = true;
                            timer.Elapsed += elapsed;
                            _renderer = renderer;
                            _swed = swed;
                            _gameRules = gameRules;
                            timer.Start();

                        }

                    }
                    else
                    {
                        renderer.timeLeft = -1;
                        renderer.bombPlanted = false;
                    }
                }
                if (renderer.enablemoneyChecking)
                {
                    nentities.Clear();
                    int _team = swed.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);
                    for (int i = 0; i < 64; i++)
                    {
                        if (listEntry == IntPtr.Zero)
                            continue;

                        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

                        if (currentController == IntPtr.Zero)
                            continue;

                        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);
                        if (pawnHandle == 0)
                            continue;

                        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                        if (listEntry2 == IntPtr.Zero)
                            continue;

                        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));
                        if (currentPawn == IntPtr.Zero)
                            continue;

                        IntPtr MoneyServices = swed.ReadPointer(currentController, Offsets.m_pInGameMoneyServices);

                        if (MoneyServices == IntPtr.Zero)
                            continue;

                        Entity entity = new Entity
                        {
                            team = swed.ReadInt(currentPawn, Offsets.m_iTeamNum),
                            _name = swed.ReadString(currentController, Offsets.m_iszPlayerName, 16),
                            account = swed.ReadInt(MoneyServices, Offsets.m_iAccount),
                            cashSpent = swed.ReadInt(MoneyServices, Offsets.m_iCashSpentThisRound),
                            cashSpentTotal = swed.ReadInt(MoneyServices, Offsets.m_iTotalCashSpent)
                        };
                        if (entity.team == _team)
                            continue;
                        nentities.Add(entity);
                    }
                    renderer.nentities = nentities;
                    Thread.Sleep(1000);
                }
                if (renderer.visibleColorChangeFOV)
                {
                    int Team = swed.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);
                    int entIndex = swed.ReadInt(localPlayerPawn, Offsets.m_iIDEntIndex);

                    if (entIndex != -1)
                    {
                        IntPtr listEntry = swed.ReadPointer(entityList, 0x8 * ((entIndex & 0x7FFF) >> 9) + 0x10);
                        IntPtr CurrentPawn = swed.ReadPointer(listEntry, 0x78 * (entIndex & 0x1FF));
                        int entityTeam = swed.ReadInt(CurrentPawn, Offsets.m_iTeamNum);

                        if (entityTeam != Team)
                        {
                            renderer.EntityVisible = true;
                        }
                        else
                        {
                            renderer.EntityVisible = false;
                        }
                    }
                    else
                    {
                        renderer.EntityVisible = false;
                    }
                }
                else
                {
                    renderer.EntityVisible = false;
                }

                if (renderer.HeadEsps || renderer.BoxEsp || renderer.BoneEsp || renderer.HealthBar || renderer.EnableTracklines || renderer.NameEsp || renderer.EnableDistanceCheck)
                {
                    espentities.Clear();
                    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);
                    EspLocalPlayer.team = swed.ReadInt(localPlayerPawn, Offsets.m_iTeamNum);
                    EspLocalPlayer.position = swed.ReadVec(localPlayerPawn, Offsets.m_vOldOrigin);
                    for (int i = 0; i < 64; i++)
                    {
                        if (listEntry == IntPtr.Zero)
                            continue;

                        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

                        if (currentController == IntPtr.Zero)
                            continue;

                        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);
                        if (pawnHandle == 0)
                            continue;

                        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
                        if (listEntry2 == IntPtr.Zero)
                            continue;

                        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));
                        if (currentPawn == IntPtr.Zero)
                            continue;

                        int lifeState = swed.ReadInt(currentPawn, Offsets.m_lifeState);
                        if (lifeState != 256) continue;

                        float[] viewMatrix = swed.ReadMatrix(client + Offsets.dwViewMatrix);

                        IntPtr sceneNode = swed.ReadPointer(currentPawn, Offsets.m_pGameSceneNode);
                        IntPtr boneMatrix = swed.ReadPointer(sceneNode, Offsets.m_modelState + 0x80);

                        Entity entity = new Entity();
                        entity.team = swed.ReadInt(currentPawn, Offsets.m_iTeamNum);
                        entity._name = swed.ReadString(currentController, Offsets.m_iszPlayerName, 16).Split("\0")[0];
                        entity.health = swed.ReadInt(currentPawn, Offsets.m_iHealth);
                        entity.position = swed.ReadVec(currentPawn, Offsets.m_vOldOrigin);
                        entity.viewOffset = swed.ReadVec(currentPawn, Offsets.m_vecViewOffset);
                        entity.position2D = Calculate.boxWorldToScreen(viewMatrix, entity.position, screenSize);
                        entity.distance = Vector3.Distance(entity.position, EspLocalPlayer.position);
                        entity.bones = Calculate.ReadBones(boneMatrix, swed);
                        entity.spotted = swed.ReadBool(currentPawn, Offsets.m_entitySpottedState + Offsets.m_bSpotted);
                        entity.bones2D = Calculate.ReadBones2D(entity.bones, viewMatrix, screenSize);
                        entity.viewPosition2D = Calculate.boxWorldToScreen(viewMatrix, Vector3.Add(entity.position, entity.viewOffset), screenSize);
                        espentities.Add(entity);
                    }
                    renderer.UpdateLocalPlayer(EspLocalPlayer);
                    renderer.UpdateEntities(espentities);
                }
            }
            ViewMatrix ReadMatrix(IntPtr matrixAddress)
            {
                var viewMatrix = new ViewMatrix();
                var matrix = swed.ReadMatrix(matrixAddress);

                viewMatrix.m11 = matrix[0];
                viewMatrix.m12 = matrix[1];
                viewMatrix.m13 = matrix[2];
                viewMatrix.m14 = matrix[3];

                viewMatrix.m21 = matrix[4];
                viewMatrix.m22 = matrix[5];
                viewMatrix.m23 = matrix[6];
                viewMatrix.m24 = matrix[7];

                viewMatrix.m31 = matrix[8];
                viewMatrix.m32 = matrix[9];
                viewMatrix.m33 = matrix[10];
                viewMatrix.m34 = matrix[11];

                viewMatrix.m41 = matrix[12];
                viewMatrix.m42 = matrix[13];
                viewMatrix.m43 = matrix[14];
                viewMatrix.m44 = matrix[15];

                return viewMatrix;
            }
        }

        static Renderer _renderer;
        static Swed _swed;
        static nint _gameRules;

        private static void elapsed(object? sender, ElapsedEventArgs e)
        {

            _renderer.bombPlanted = _swed.ReadBool(_gameRules, Offsets.m_bBombPlanted);
            if (!_renderer.bombPlanted)
                timer.Stop();
            int timeLeft = 40 - 1;

            _renderer.timeLeft = timeLeft;
            _renderer.bombPlanted = true;
        }
    }
}
