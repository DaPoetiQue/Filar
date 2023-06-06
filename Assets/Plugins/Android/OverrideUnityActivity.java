package com.redicalgames.designar;
import android.os.Bundle;
import android.widget.FrameLayout;

import com.unity3d.player.UnityPlayerActivity;

public abstract class OverrideUnityActivity extends UnityPlayerActivity
{
    public static OverrideUnityActivity instance = null;

    abstract protected String GetAppDataDirectory(String path);

    abstract protected void RequestAppPermissions();

    abstract protected void SelectOBJAssetFile();
    
    abstract protected void SelectThumbnailAssetFile();

    abstract protected void SelectMainTextureAssetFile();
    abstract protected void SelectNormalMapAssetFile();
    abstract protected void SelectAOMapAssetFile();

    abstract protected void StartVoiceCommands();

    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        instance = this;
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();
        instance = null;
    }
}