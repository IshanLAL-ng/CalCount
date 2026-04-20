using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace CalCount.View;

public partial class PongPage : ContentPage
{
    double leftY = 0, rightY = 0;
    double ballX = 0, ballY = 0;
    double ballVX = 3, ballVY = 2;
    double paddleHeight = 60;
    IDispatcherTimer? timer;

    int leftScore = 0, rightScore = 0;
    double baseSpeed = 3; // controlled by level

    public PongPage()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    void OnSizeChanged(object? sender, EventArgs e)
    {
        // Position paddles and ball
        leftY = (PlayArea.Height - paddleHeight) / 2;
        rightY = leftY;
        ballX = (PlayArea.Width - Ball.Width) / 2;
        ballY = (PlayArea.Height - Ball.Height) / 2;
        UpdateLayoutPositions();

        if (timer == null)
        {
            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += OnTick;
            timer.Start();
        }

        // default level (Normal)
        baseSpeed = 3;
    }

    void UpdateLayoutPositions()
    {
        AbsoluteLayout.SetLayoutBounds(LeftPaddle, new Rect(0, leftY, LeftPaddle.Width, paddleHeight));
        AbsoluteLayout.SetLayoutBounds(RightPaddle, new Rect(PlayArea.Width - RightPaddle.Width, rightY, RightPaddle.Width, paddleHeight));
        AbsoluteLayout.SetLayoutBounds(Ball, new Rect(ballX, ballY, Ball.Width, Ball.Height));
    }

    void OnTick(object? sender, EventArgs e)
    {
        ballX += ballVX;
        ballY += ballVY;

        // Top/bottom collision
        if (ballY <= 0 || ballY + Ball.Height >= PlayArea.Height) ballVY = -ballVY;

        // Left paddle collision
        if (ballX <= LeftPaddle.Width)
        {
            if (ballY + Ball.Height >= leftY && ballY <= leftY + paddleHeight)
            {
                ballVX = Math.Abs(ballVX);
            }
            else
            {
                // right player scores
                rightScore++;
                RightScoreLabel.Text = $"Right: {rightScore}";
                ResetBall(centerRight: true);
            }
        }

        // Right paddle collision
        if (ballX + Ball.Width >= PlayArea.Width - RightPaddle.Width)
        {
            if (ballY + Ball.Height >= rightY && ballY <= rightY + paddleHeight)
            {
                ballVX = -Math.Abs(ballVX);
            }
            else
            {
                // left player scores
                leftScore++;
                LeftScoreLabel.Text = $"Left: {leftScore}";
                ResetBall(centerRight: false);
            }
        }

        UpdateLayoutPositions();
    }

    void ResetBall(bool centerRight)
    {
        ballX = (PlayArea.Width - Ball.Width) / 2;
        ballY = (PlayArea.Height - Ball.Height) / 2;
        // set speed according to level
        ballVX = centerRight ? Math.Abs(baseSpeed) : -Math.Abs(baseSpeed);
        // small random vertical direction
        var sign = (new Random().Next(0, 2) == 0) ? -1 : 1;
        ballVY = sign * baseSpeed * 0.6;
    }

    void OnP1Up(object sender, EventArgs e)
    {
        leftY = Math.Max(0, leftY - 20);
        UpdateLayoutPositions();
    }

    void OnP1Down(object sender, EventArgs e)
    {
        leftY = Math.Min(PlayArea.Height - paddleHeight, leftY + 20);
        UpdateLayoutPositions();
    }

    void OnP2Up(object sender, EventArgs e)
    {
        rightY = Math.Max(0, rightY - 20);
        UpdateLayoutPositions();
    }

    void OnP2Down(object sender, EventArgs e)
    {
        rightY = Math.Min(PlayArea.Height - paddleHeight, rightY + 20);
        UpdateLayoutPositions();
    }

    void OnLevelEasy(object sender, EventArgs e)
    {
        baseSpeed = 2;
        ballVX = Math.Sign(ballVX) * baseSpeed;
        ballVY = Math.Sign(ballVY) * baseSpeed * 0.6;
    }

    void OnLevelNormal(object sender, EventArgs e)
    {
        baseSpeed = 3;
        ballVX = Math.Sign(ballVX) * baseSpeed;
        ballVY = Math.Sign(ballVY) * baseSpeed * 0.6;
    }

    void OnLevelHard(object sender, EventArgs e)
    {
        baseSpeed = 5;
        ballVX = Math.Sign(ballVX) * baseSpeed;
        ballVY = Math.Sign(ballVY) * baseSpeed * 0.6;
    }

#if WINDOWS
    Microsoft.UI.Xaml.UIElement? nativeElement;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            var nativeWindow = Application.Current?.Windows[0]?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            if (nativeWindow?.Content is Microsoft.UI.Xaml.UIElement content)
            {
                nativeElement = content;
                content.KeyDown += OnNativeKeyDown;
            }
        }
        catch { }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
        try { if (nativeElement != null) nativeElement.KeyDown -= OnNativeKeyDown; } catch { }
    }

    void OnNativeKeyDown(object? sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        var key = e.Key;
        // WASD for left paddle, arrow keys for right paddle
        if (key == Windows.System.VirtualKey.W)
        {
            leftY = Math.Max(0, leftY - 20);
            UpdateLayoutPositions();
        }
        else if (key == Windows.System.VirtualKey.S)
        {
            leftY = Math.Min(PlayArea.Height - paddleHeight, leftY + 20);
            UpdateLayoutPositions();
        }
        else if (key == Windows.System.VirtualKey.Up)
        {
            rightY = Math.Max(0, rightY - 20);
            UpdateLayoutPositions();
        }
        else if (key == Windows.System.VirtualKey.Down)
        {
            rightY = Math.Min(PlayArea.Height - paddleHeight, rightY + 20);
            UpdateLayoutPositions();
        }
    }
#else
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
    }
#endif
}
