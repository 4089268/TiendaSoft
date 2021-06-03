from flask import Flask, send_file
import pypyodbc
from flask_cors import CORS
from io import BytesIO
import pathlib # libreria para utilizar directorios
import os


cadenaConexion = 'Driver={SQL Server};Server=nerus.sytes.net\SQL2017;Database=pdv;uid=usr_arquos;pwd=usr_arquos;'

app = Flask(__name__)
CORS(app)
@app.route('/')
def defautl():
    return "MiTiendita API, Version 1.1.0"

@app.route('/productos')
def productos():

    conexion = pypyodbc.connect(cadenaConexion)
    cursor = conexion.cursor()
    query = "SELECT id_producto, descripcion, existencia, precio_v FROM dbo.Opr_productos "

    cursor.execute(query)    
    result = []
    while True:
        row = cursor.fetchone()
        if not row:
            break
        xr = { 'Id':str(row['id_producto']), 'Descripcion':str(row['descripcion']), 'Existencia':str(row['existencia']), 'Precio':str(row['precio_v'])}
        result.append(xr)
    cursor.close()
    conexion.close()

    return {'productos':result}
    
@app.route('/busqueda/<palabra>')
def show_user_profile(palabra):
    conexion = pypyodbc.connect(cadenaConexion)
    cursor = conexion.cursor()
    query = "SELECT id_producto, descripcion, existencia, precio_v FROM dbo.Opr_productos WHERE DESCRIPCION LIKE '%" + palabra + "%'"
    cursor.execute(query)
    
    result = []
    while True:
        row = cursor.fetchone()
        if not row:
            break
        xr = { 'Id':str(row['id_producto']), 'Descripcion':str(row['descripcion']), 'Existencia':str(row['existencia']), 'Precio':str(row['precio_v'])  }
        result.append(xr)
    cursor.close()
    conexion.close()

    return {'productos':result}


@app.route('/descargarImage/<id>')
def DescargarImagem(id):

    conexion = pypyodbc.connect(cadenaConexion)
    cursor = conexion.cursor()
    query = "Select id_producto, foto1 From  [dbo].[Opr_productos] Where id_producto = " + id
    cursor.execute(query)

    row = cursor.fetchone()
    data = row["foto1"]
        
    cursor.close()
    conexion.close()

    return send_file(BytesIO(data),attachment_filename="img.jpg",as_attachment=True)

@app.route('/imagen/<id>')
def Imagen(id):

    conexion = pypyodbc.connect(cadenaConexion)
    cursor = conexion.cursor()
    query = "Select id_producto, foto1 From  [dbo].[Opr_productos] Where id_producto = " + id + "and not foto1 is null"
    cursor.execute(query)

    # row = cursor.fetchone()
    # imagen = row["foto1"]

    rowsData = cursor.fetchall()
    if len(rowsData) > 0:        
        imagen = rowsData[0]["foto1"]
        imageBytes = BytesIO(imagen)
    else:
        # Si no se encuentra la el producto o el producto no tiene imagen, mostrar una imagen temporal
        noImagePath = os.getcwd() + "\\res\\noimage.png"
        fileReader = open( noImagePath , "rb") # opening for [r]eading as [b]inary
        image = fileReader.read()
        imageBytes = BytesIO(image)
        fileReader.close()

    cursor.close()
    conexion.close()

    return send_file(imageBytes, mimetype='image/gif') 