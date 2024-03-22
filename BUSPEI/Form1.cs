using NAudio.Wave;

namespace BUSPEI
{
    public partial class Form1 : Form
    {
        private WaveOut waveOut = new WaveOut();

        public Form1()
        {
            InitializeComponent();
        }

        private void playPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 再生設定
            AudioFileReader reader = new AudioFileReader(@"./bus.mp3");
            LoopStream loop = new LoopStream(reader);
            waveOut.Init(loop);
            waveOut.Play();
        }

        private void stopMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 停止設定
            waveOut.Dispose();
        }
    }

    // ループクラス
    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        public bool EnableLooping { get; set; }

        public override WaveFormat WaveFormat
        {
            get { return sourceStream.WaveFormat; }
        }

        public override long Length
        {
            get { return sourceStream.Length; }
        }

        public override long Position
        {
            get { return sourceStream.Position; }
            set { sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        break;
                    }
                    // loop
                    sourceStream.Position = 0;
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}
