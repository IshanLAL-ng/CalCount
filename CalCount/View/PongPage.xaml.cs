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
                ResetBall(centerRight: false);
            }
        }

        UpdateLayoutPositions();
    }

    void ResetBall(bool centerRight)
    {
        ballX = (PlayArea.Width - Ball.Width) / 2;
        ballY = (PlayArea.Height - Ball.Height) / 2;
        ballVX = centerRight ? Math.Abs(ballVX) : -Math.Abs(ballVX);
        ballVY = 2;
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (timer != null)
        {
            timer.Stop();
            timer = null;
        }
    }
}
