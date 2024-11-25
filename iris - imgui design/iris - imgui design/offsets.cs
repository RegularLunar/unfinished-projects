//CoDWaW.exe + 1DC4F98
// "plutonium-bootstrapper-win32.exe"+1DB5D60

/*
Renderer renderer = new Renderer();
Thread renderThread = new Thread(new ThreadStart(renderer.Start().Wait));
renderThread.Start();

Swed swed = new Swed("plutonium-bootstrapper-win32");

IntPtr baseModule = swed.GetModuleBase("plutonium-bootstrapper-win32.exe");
int fovAddress = 0x1DB5D60;
while (true)
{
    swed.WriteFloat(baseModule, fovAddress,renderer.fov);
    Thread.Sleep(10);
}

/*
MemScanner memeScan = new MemScanner(swed.GetProcess());

List<IntPtr> results = memeScan.ScanMemory("F3 0F 10 50 10 F3 0F 5F F0 F3 0F 11 35 FD 0A 7D 00");

foreach (IntPtr address in results)
{
    Console.WriteLine(address.ToString("X"));
}
*/