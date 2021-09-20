<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmKoneksi
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmKoneksi))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnSimpan = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtDatabaseHO = New System.Windows.Forms.TextBox()
        Me.txtPasswordHO = New System.Windows.Forms.TextBox()
        Me.txtLoginHO = New System.Windows.Forms.TextBox()
        Me.txtServerNameHO = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.txtDatabaseLoc = New System.Windows.Forms.TextBox()
        Me.txtPasswordLoc = New System.Windows.Forms.TextBox()
        Me.txtLoginLoc = New System.Windows.Forms.TextBox()
        Me.txtServerNameLoc = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnForceUpdate = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnSimpan)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(864, 266)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'btnSimpan
        '
        Me.btnSimpan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSimpan.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSimpan.Image = CType(resources.GetObject("btnSimpan.Image"), System.Drawing.Image)
        Me.btnSimpan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSimpan.Location = New System.Drawing.Point(726, 218)
        Me.btnSimpan.Name = "btnSimpan"
        Me.btnSimpan.Size = New System.Drawing.Size(122, 41)
        Me.btnSimpan.TabIndex = 114
        Me.btnSimpan.Text = "Simpan"
        Me.btnSimpan.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSimpan.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Button1)
        Me.GroupBox3.Controls.Add(Me.txtDatabaseHO)
        Me.GroupBox3.Controls.Add(Me.txtPasswordHO)
        Me.GroupBox3.Controls.Add(Me.txtLoginHO)
        Me.GroupBox3.Controls.Add(Me.txtServerNameHO)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.Label9)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Location = New System.Drawing.Point(430, 19)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(418, 193)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "SERVER HO"
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(314, 148)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(84, 33)
        Me.Button1.TabIndex = 115
        Me.Button1.Text = "Test"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtDatabaseHO
        '
        Me.txtDatabaseHO.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDatabaseHO.Location = New System.Drawing.Point(167, 109)
        Me.txtDatabaseHO.Name = "txtDatabaseHO"
        Me.txtDatabaseHO.Size = New System.Drawing.Size(231, 24)
        Me.txtDatabaseHO.TabIndex = 8
        '
        'txtPasswordHO
        '
        Me.txtPasswordHO.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPasswordHO.Location = New System.Drawing.Point(167, 79)
        Me.txtPasswordHO.Name = "txtPasswordHO"
        Me.txtPasswordHO.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPasswordHO.Size = New System.Drawing.Size(231, 24)
        Me.txtPasswordHO.TabIndex = 7
        Me.txtPasswordHO.UseSystemPasswordChar = True
        '
        'txtLoginHO
        '
        Me.txtLoginHO.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoginHO.Location = New System.Drawing.Point(167, 49)
        Me.txtLoginHO.Name = "txtLoginHO"
        Me.txtLoginHO.Size = New System.Drawing.Size(231, 24)
        Me.txtLoginHO.TabIndex = 6
        '
        'txtServerNameHO
        '
        Me.txtServerNameHO.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServerNameHO.Location = New System.Drawing.Point(167, 19)
        Me.txtServerNameHO.Name = "txtServerNameHO"
        Me.txtServerNameHO.Size = New System.Drawing.Size(231, 24)
        Me.txtServerNameHO.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(32, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 13)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Server Name"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(32, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Login / User"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(32, 116)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(53, 13)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Database"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(32, 86)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(53, 13)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Password"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.txtDatabaseLoc)
        Me.GroupBox2.Controls.Add(Me.txtPasswordLoc)
        Me.GroupBox2.Controls.Add(Me.txtLoginLoc)
        Me.GroupBox2.Controls.Add(Me.txtServerNameLoc)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 19)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(418, 193)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "SERVER LOKAL"
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(314, 148)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(84, 33)
        Me.Button2.TabIndex = 116
        Me.Button2.Text = "Test"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = True
        '
        'txtDatabaseLoc
        '
        Me.txtDatabaseLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDatabaseLoc.Location = New System.Drawing.Point(167, 109)
        Me.txtDatabaseLoc.Name = "txtDatabaseLoc"
        Me.txtDatabaseLoc.Size = New System.Drawing.Size(231, 24)
        Me.txtDatabaseLoc.TabIndex = 8
        '
        'txtPasswordLoc
        '
        Me.txtPasswordLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPasswordLoc.Location = New System.Drawing.Point(167, 79)
        Me.txtPasswordLoc.Name = "txtPasswordLoc"
        Me.txtPasswordLoc.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPasswordLoc.Size = New System.Drawing.Size(231, 24)
        Me.txtPasswordLoc.TabIndex = 7
        Me.txtPasswordLoc.UseSystemPasswordChar = True
        '
        'txtLoginLoc
        '
        Me.txtLoginLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLoginLoc.Location = New System.Drawing.Point(167, 49)
        Me.txtLoginLoc.Name = "txtLoginLoc"
        Me.txtLoginLoc.Size = New System.Drawing.Size(231, 24)
        Me.txtLoginLoc.TabIndex = 6
        '
        'txtServerNameLoc
        '
        Me.txtServerNameLoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtServerNameLoc.Location = New System.Drawing.Point(167, 19)
        Me.txtServerNameLoc.Name = "txtServerNameLoc"
        Me.txtServerNameLoc.Size = New System.Drawing.Size(231, 24)
        Me.txtServerNameLoc.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Server Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(32, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Login / User"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(32, 116)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Database"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(32, 86)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Password"
        '
        'btnForceUpdate
        '
        Me.btnForceUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForceUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForceUpdate.Image = CType(resources.GetObject("btnForceUpdate.Image"), System.Drawing.Image)
        Me.btnForceUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForceUpdate.Location = New System.Drawing.Point(695, 233)
        Me.btnForceUpdate.Name = "btnForceUpdate"
        Me.btnForceUpdate.Size = New System.Drawing.Size(153, 41)
        Me.btnForceUpdate.TabIndex = 114
        Me.btnForceUpdate.Text = "Simpan"
        Me.btnForceUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnForceUpdate.UseVisualStyleBackColor = True
        '
        'FrmKoneksi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 303)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "FrmKoneksi"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Connect To Database"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDatabaseHO As System.Windows.Forms.TextBox
    Friend WithEvents txtPasswordHO As System.Windows.Forms.TextBox
    Friend WithEvents txtLoginHO As System.Windows.Forms.TextBox
    Friend WithEvents txtServerNameHO As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtDatabaseLoc As System.Windows.Forms.TextBox
    Friend WithEvents txtPasswordLoc As System.Windows.Forms.TextBox
    Friend WithEvents txtLoginLoc As System.Windows.Forms.TextBox
    Friend WithEvents txtServerNameLoc As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnSimpan As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnForceUpdate As System.Windows.Forms.Button
End Class
