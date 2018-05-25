<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ResourcePackLocation = New System.Windows.Forms.TextBox()
        Me.ResourcePackLocationLabel = New System.Windows.Forms.Label()
        Me.BrowseResourcePackLocation = New System.Windows.Forms.Button()
        Me.ResourcePackNameLabel = New System.Windows.Forms.Label()
        Me.ResourcePackName = New System.Windows.Forms.TextBox()
        Me.BookCommandBox = New System.Windows.Forms.TextBox()
        Me.GenerateCommandButton = New System.Windows.Forms.Button()
        Me.BookNameLabel = New System.Windows.Forms.Label()
        Me.BookName = New System.Windows.Forms.TextBox()
        Me.AuthorNameLabel = New System.Windows.Forms.Label()
        Me.AuthorName = New System.Windows.Forms.TextBox()
        Me.NamespaceLabel = New System.Windows.Forms.Label()
        Me.NamespaceBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ResourcePackLocation
        '
        Me.ResourcePackLocation.Location = New System.Drawing.Point(143, 6)
        Me.ResourcePackLocation.Name = "ResourcePackLocation"
        Me.ResourcePackLocation.ReadOnly = True
        Me.ResourcePackLocation.Size = New System.Drawing.Size(265, 20)
        Me.ResourcePackLocation.TabIndex = 1
        Me.ResourcePackLocation.Text = "F:\My Minecraft Expansion\.minecraft\resourcepacks"
        Me.ResourcePackLocation.WordWrap = False
        '
        'ResourcePackLocationLabel
        '
        Me.ResourcePackLocationLabel.AutoSize = True
        Me.ResourcePackLocationLabel.Location = New System.Drawing.Point(9, 9)
        Me.ResourcePackLocationLabel.Name = "ResourcePackLocationLabel"
        Me.ResourcePackLocationLabel.Size = New System.Drawing.Size(128, 13)
        Me.ResourcePackLocationLabel.TabIndex = 1
        Me.ResourcePackLocationLabel.Text = "Resource Pack Location:"
        '
        'BrowseResourcePackLocation
        '
        Me.BrowseResourcePackLocation.Location = New System.Drawing.Point(414, 6)
        Me.BrowseResourcePackLocation.Name = "BrowseResourcePackLocation"
        Me.BrowseResourcePackLocation.Size = New System.Drawing.Size(58, 20)
        Me.BrowseResourcePackLocation.TabIndex = 2
        Me.BrowseResourcePackLocation.Text = "Browse"
        Me.BrowseResourcePackLocation.UseVisualStyleBackColor = True
        '
        'ResourcePackNameLabel
        '
        Me.ResourcePackNameLabel.AutoSize = True
        Me.ResourcePackNameLabel.Location = New System.Drawing.Point(9, 35)
        Me.ResourcePackNameLabel.Name = "ResourcePackNameLabel"
        Me.ResourcePackNameLabel.Size = New System.Drawing.Size(115, 13)
        Me.ResourcePackNameLabel.TabIndex = 3
        Me.ResourcePackNameLabel.Text = "Resource Pack Name:"
        '
        'ResourcePackName
        '
        Me.ResourcePackName.Location = New System.Drawing.Point(130, 32)
        Me.ResourcePackName.Name = "ResourcePackName"
        Me.ResourcePackName.Size = New System.Drawing.Size(135, 20)
        Me.ResourcePackName.TabIndex = 3
        Me.ResourcePackName.Text = "ZeroServerSoundPack12"
        Me.ResourcePackName.WordWrap = False
        '
        'BookCommandBox
        '
        Me.BookCommandBox.Location = New System.Drawing.Point(12, 110)
        Me.BookCommandBox.Multiline = True
        Me.BookCommandBox.Name = "BookCommandBox"
        Me.BookCommandBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.BookCommandBox.Size = New System.Drawing.Size(460, 140)
        Me.BookCommandBox.TabIndex = 7
        '
        'GenerateCommandButton
        '
        Me.GenerateCommandButton.Location = New System.Drawing.Point(395, 57)
        Me.GenerateCommandButton.Name = "GenerateCommandButton"
        Me.GenerateCommandButton.Size = New System.Drawing.Size(77, 47)
        Me.GenerateCommandButton.TabIndex = 0
        Me.GenerateCommandButton.Text = "Generate Book Command"
        Me.GenerateCommandButton.UseVisualStyleBackColor = True
        '
        'BookNameLabel
        '
        Me.BookNameLabel.AutoSize = True
        Me.BookNameLabel.Location = New System.Drawing.Point(9, 61)
        Me.BookNameLabel.Name = "BookNameLabel"
        Me.BookNameLabel.Size = New System.Drawing.Size(66, 13)
        Me.BookNameLabel.TabIndex = 7
        Me.BookNameLabel.Text = "Book Name:"
        '
        'BookName
        '
        Me.BookName.Location = New System.Drawing.Point(81, 58)
        Me.BookName.Name = "BookName"
        Me.BookName.Size = New System.Drawing.Size(308, 20)
        Me.BookName.TabIndex = 5
        Me.BookName.Text = "MusicControlBook"
        Me.BookName.WordWrap = False
        '
        'AuthorNameLabel
        '
        Me.AuthorNameLabel.AutoSize = True
        Me.AuthorNameLabel.Location = New System.Drawing.Point(9, 87)
        Me.AuthorNameLabel.Name = "AuthorNameLabel"
        Me.AuthorNameLabel.Size = New System.Drawing.Size(72, 13)
        Me.AuthorNameLabel.TabIndex = 9
        Me.AuthorNameLabel.Text = "Author Name:"
        '
        'AuthorName
        '
        Me.AuthorName.Location = New System.Drawing.Point(87, 84)
        Me.AuthorName.Name = "AuthorName"
        Me.AuthorName.Size = New System.Drawing.Size(302, 20)
        Me.AuthorName.TabIndex = 6
        Me.AuthorName.Text = "zero318"
        Me.AuthorName.WordWrap = False
        '
        'NamespaceLabel
        '
        Me.NamespaceLabel.AutoSize = True
        Me.NamespaceLabel.Location = New System.Drawing.Point(271, 35)
        Me.NamespaceLabel.Name = "NamespaceLabel"
        Me.NamespaceLabel.Size = New System.Drawing.Size(67, 13)
        Me.NamespaceLabel.TabIndex = 10
        Me.NamespaceLabel.Text = "Namespace:"
        '
        'NamespaceBox
        '
        Me.NamespaceBox.Location = New System.Drawing.Point(344, 32)
        Me.NamespaceBox.Name = "NamespaceBox"
        Me.NamespaceBox.Size = New System.Drawing.Size(128, 20)
        Me.NamespaceBox.TabIndex = 4
        Me.NamespaceBox.Text = "derpcraft"
        Me.NamespaceBox.WordWrap = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(484, 262)
        Me.Controls.Add(Me.NamespaceBox)
        Me.Controls.Add(Me.NamespaceLabel)
        Me.Controls.Add(Me.AuthorName)
        Me.Controls.Add(Me.AuthorNameLabel)
        Me.Controls.Add(Me.BookName)
        Me.Controls.Add(Me.BookNameLabel)
        Me.Controls.Add(Me.GenerateCommandButton)
        Me.Controls.Add(Me.BookCommandBox)
        Me.Controls.Add(Me.ResourcePackName)
        Me.Controls.Add(Me.ResourcePackNameLabel)
        Me.Controls.Add(Me.BrowseResourcePackLocation)
        Me.Controls.Add(Me.ResourcePackLocationLabel)
        Me.Controls.Add(Me.ResourcePackLocation)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "Minecraft Music System Command Generator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents ResourcePackLocation As TextBox
    Friend WithEvents ResourcePackLocationLabel As Label
    Friend WithEvents BrowseResourcePackLocation As Button
    Friend WithEvents ResourcePackNameLabel As Label
    Friend WithEvents ResourcePackName As TextBox
    Friend WithEvents BookCommandBox As TextBox
    Friend WithEvents GenerateCommandButton As Button
    Friend WithEvents BookNameLabel As Label
    Friend WithEvents BookName As TextBox
    Friend WithEvents AuthorNameLabel As Label
    Friend WithEvents AuthorName As TextBox
    Friend WithEvents NamespaceLabel As Label
    Friend WithEvents NamespaceBox As TextBox
End Class
