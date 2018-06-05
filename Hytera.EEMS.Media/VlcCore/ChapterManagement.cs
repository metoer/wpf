
namespace Hytera.EEMS.Media
{
    internal sealed class ChapterManagement : IChapterManagement
    {
        private readonly VlcManager myManager;
        private readonly VlcMediaPlayerInstance myMediaPlayer;

        public ChapterManagement(VlcManager manager, VlcMediaPlayerInstance mediaPlayerInstance)
        {
            myManager = manager;
            myMediaPlayer = mediaPlayerInstance;
        }

        public int Count
        {
            get { return myManager.GetMediaChapterCount(myMediaPlayer); }
        }

        public void Previous()
        {
            myManager.SetPreviousMediaChapter(myMediaPlayer);
        }

        public void Next()
        {
            myManager.SetNextMediaChapter(myMediaPlayer);
        }

        public int Current
        {
            get { return myManager.GetMediaChapter(myMediaPlayer); }
            set { myManager.SetMediaChapter(myMediaPlayer, value); }
        }
    }
}
