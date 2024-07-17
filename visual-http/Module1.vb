Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Diagnostics

Module Module1

    Sub Main()
        Console.WriteLine("Loading Config File....")
        Dim port As String = "8080"
        Dim errorcontent As String = "<html><body><h1>404 - File Not Found</h1><br><p>powered by visual http</p></body></html>"
        If File.Exists("config.yml") Then
            Dim configLines() As String = File.ReadAllLines("config.yml")
            For Each line As String In configLines
                If line.StartsWith("port=") Then
                    port = line.Substring(5)
                End If
                If line.StartsWith("error=") Then
                    errorcontent = line.Substring(6)
                End If
            Next
        End If
        Console.WriteLine("Port As " + port)
        Console.WriteLine("Error As " + errorcontent)
        Dim listener As New HttpListener()
        listener.Prefixes.Add("http://localhost:" + port + "/")
        listener.Start()
        Console.WriteLine("Listening on Port " + port)

        While True
            Dim context As HttpListenerContext = listener.GetContext()
            Dim request As HttpListenerRequest = context.Request
            Dim response As HttpListenerResponse = context.Response

            Dim filePath As String = request.Url.LocalPath.TrimStart("/"c)
            If String.IsNullOrEmpty(filePath) Then
                filePath = "index.html"
            End If
            If Not filePath.Contains(".html") Then
                filePath = String.Concat(filePath, "/index.html")
            End If
            If File.Exists(filePath) Then
                Dim buffer() As Byte = File.ReadAllBytes(filePath)
                response.ContentLength64 = buffer.Length
                Dim output As Stream = response.OutputStream
                output.Write(buffer, 0, buffer.Length)
                output.Close()
                Console.WriteLine("Connection recieved, file " + filePath + " displayed.")
            Else
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(errorcontent)
                response.ContentLength64 = buffer.Length
                Dim output As Stream = response.OutputStream
                output.Write(buffer, 0, buffer.Length)
                output.Close()
            End If
        End While
    End Sub

End Module
