using System;
using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public partial class HDAdditionalCameraData : IVersionable<HDAdditionalCameraData.Version>
    {
        protected enum Version
        {
            None,
            First,
            SeparatePassThrough,
            UpgradingFrameSettingsToStruct
        }

        [SerializeField, FormerlySerializedAs("version")]
        Version m_Version;

        protected static readonly MigrationDescription<Version, HDAdditionalCameraData> k_Migration = MigrationDescription.New(
            MigrationStep.New(Version.SeparatePassThrough, (HDAdditionalCameraData data) =>
            {
#pragma warning disable 618 // Type or member is obsolete
                switch ((int)data.m_ObsoleteRenderingPath)
#pragma warning restore 618
                {
                    case 0: //former RenderingPath.UseGraphicsSettings
                        data.fullscreenPassthrough = false;
                        data.customRenderingSettings = false;
                        break;
                    case 1: //former RenderingPath.Custom
                        data.fullscreenPassthrough = false;
                        data.customRenderingSettings = true;
                        break;
                    case 2: //former RenderingPath.FullscreenPassthrough
                        data.fullscreenPassthrough = true;
                        data.customRenderingSettings = false;
                        break;
                }
            }),
            MigrationStep.New(Version.UpgradingFrameSettingsToStruct, (HDAdditionalCameraData data) =>
            {
#pragma warning disable 618 // Type or member is obsolete
                if (data.m_ObsoleteFrameSettings != null)
                    FrameSettings.MigrateFromClassVersion(ref data.m_ObsoleteFrameSettings, ref data.m_RenderingPathCustomFrameSettings, ref data.m_RenderingPathCustomOverrideFrameSettings);
#pragma warning restore 618
            })
        );

        Version IVersionable<Version>.version { get => m_Version; set => m_Version = value; }

#pragma warning disable 649 // Field never assigned
        [SerializeField, FormerlySerializedAs("renderingPath"), Obsolete("For Data Migration")]
        int m_ObsoleteRenderingPath;
        [SerializeField]
        [FormerlySerializedAs("serializedFrameSettings"), FormerlySerializedAs("m_FrameSettings")]
#pragma warning disable 618 // Type or member is obsolete
        ObsoleteFrameSettings m_ObsoleteFrameSettings;
#pragma warning restore 618
#pragma warning restore 649
    }
}
