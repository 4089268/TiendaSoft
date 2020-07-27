from flask import Flask
import pypyodbc
from flask_cors import CORS
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

    tmpUrlImage="https://www.freeiconspng.com/uploads/no-image-icon-10.png"
    result = []
    while True:
        row = cursor.fetchone()
        if not row:
            break
        xr = { 'Id':str(row['id_producto']), 'Descripcion':str(row['descripcion']), 'Existencia':str(row['existencia']), 'Precio':str(row['precio_v']), 'ImagenUrl':tmpUrlImage}
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
    
    tmpUrlImage="https://www.freeiconspng.com/uploads/no-image-icon-10.png"
    result = []
    while True:
        row = cursor.fetchone()
        if not row:
            break
        xr = { 'Id':str(row['id_producto']), 'Descripcion':str(row['descripcion']), 'Existencia':str(row['existencia']), 'Precio':str(row['precio_v']), 'ImagenUrl':tmpUrlImage}
        result.append(xr)
    cursor.close()
    conexion.close()

    return {'productos':result}