Imports System
Imports System.Drawing
Imports System.Windows.Forms

Friend Class PicturePanel
    Inherits Panel

    Public Sub New()
        Me.DoubleBuffered = True
        Me.AutoScroll = True
        Me.BackgroundImageLayout = ImageLayout.Center
    End Sub

    Public Overrides Property BackgroundImage As Image
        Get
            Return MyBase.BackgroundImage
        End Get
        Set(ByVal value As Image)
            MyBase.BackgroundImage = value
            If value IsNot Nothing Then Me.AutoScrollMinSize = value.Size
        End Set
    End Property
End Class