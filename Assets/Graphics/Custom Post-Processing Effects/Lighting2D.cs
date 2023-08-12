using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(Lighting2DRenderer), PostProcessEvent.BeforeStack, "Custom/2D Lighting")]
public sealed class Lighting2D : PostProcessEffectSettings
{
    public TextureParameter lightmap = new TextureParameter()
    {
        defaultState = TextureParameterDefault.Black
    };
    public override bool IsEnabledAndSupported(PostProcessRenderContext context) => enabled.value && lightmap != null;
}
public sealed class Lighting2DRenderer : PostProcessEffectRenderer<Lighting2D>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Lighting2D"));
        if (settings.lightmap != null) sheet.properties.SetTexture("_Lightmap", settings.lightmap);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
