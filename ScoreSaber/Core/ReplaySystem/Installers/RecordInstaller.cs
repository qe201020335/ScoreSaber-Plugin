﻿using Zenject;
using ScoreSaber.Core.ReplaySystem.Recorders;
using SiraUtil.Logging;

namespace ScoreSaber.Core.ReplaySystem.Installers
{
    internal class RecordInstaller : Installer
    {
        private readonly SiraLog _siraLog;
        private readonly GameplayCoreSceneSetupData _gameplayCoreSceneSetupData;

        public RecordInstaller(SiraLog siraLog, GameplayCoreSceneSetupData gameplayCoreSceneSetupData) {

            _siraLog = siraLog;
            _gameplayCoreSceneSetupData = gameplayCoreSceneSetupData;
        }
        public override void InstallBindings() {

            bool hasV3Stuff = ContainsV3Stuff();

            if (hasV3Stuff) {

                _siraLog.Warn("This map contains Beatmap V3 sliders! Not recording...");
                return;
            }

            if (!Plugin.ReplayState.isPlaybackEnabled) {

                Container.BindInterfacesAndSelfTo<Recorder>().AsSingle();
                Container.BindInterfacesAndSelfTo<MetadataRecorder>().AsSingle();
                Container.BindInterfacesAndSelfTo<HeightEventRecorder>().AsSingle();
                Container.BindInterfacesAndSelfTo<NoteEventRecorder>().AsSingle();
                Container.BindInterfacesAndSelfTo<PoseRecorder>().AsSingle();
                Container.BindInterfacesAndSelfTo<ScoreEventRecorder>().AsSingle();
                Container.BindInterfacesAndSelfTo<EnergyEventRecorder>().AsSingle();
            }
        }

        private bool ContainsV3Stuff() {

            foreach (var item in _gameplayCoreSceneSetupData.transformedBeatmapData.allBeatmapDataItems)
                if (item.type == BeatmapDataItem.BeatmapDataItemType.BeatmapObject && item is SliderData)
                    return true;
            return false;
        }
    }
}
