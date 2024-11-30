public interface IGameSystem
{
	void Initialize();
	virtual void OnSystemsInitialized() { }
	virtual void OnSceneObjectsInitialized() { }
	virtual void OnSceneReady() { }
	virtual void OnSceneStart() { }
	void Uninitialize();
}
