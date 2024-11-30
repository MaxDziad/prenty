public interface ISceneObject
{
	void OnInitialize();
	virtual void OnSceneSystemsPrepared() { }
	virtual void OnGameStart() { }
}
