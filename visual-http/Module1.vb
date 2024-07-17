Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Diagnostics

Module Module1

    Sub Main()
        Dim listener As New HttpListener()
        listener.Prefixes.Add("http://localhost:8080/")
        listener.Start()
        Console.WriteLine("Visual HTTP started on port 8080. Create an index.html file stored in the same directory, to load a page")

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
            Else
                Dim errorMessage As String = "<html><body><h1>404 - File Not Found</h1><br><p>powered by visual http</p></body></html>"
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(errorMessage)
                response.ContentLength64 = buffer.Length
                Dim output As Stream = response.OutputStream
                output.Write(buffer, 0, buffer.Length)
                output.Close()

            End If
        End While
    End Sub

End Module
