from flask import Flask
import pypyodbc
cadenaConexion = 'Driver={SQL Server};Server=nerus.sytes.net\SQL2017;Database=pdv;uid=usr_arquos;pwd=usr_arquos;'

app = Flask(__name__)

@app.route('/')
def defautl():
    return "MiTiendita API, Version 1.1.0"

@app.route('/productos')
def productos():

    conexion = pypyodbc.connect(cadenaConexion)
    cursor = conexion.cursor()
    query = "SELECT id_producto, descripcion FROM dbo.Opr_productos "

    cursor.execute(query)

    result = []
    while True:
        row = cursor.fetchone()
        if not row:
            break
        # xr = {'Id':row['id_producto'],'Descripcion':row['descripcion'],'Existencia':row['existencia'],'Precio':row['precio_v']}
        xr = {'Id':row['id_producto'], 'Descripcion':row['descripcion']}
        result.append(xr)
    cursor.close()
    conexion.close()

    return {'productos':result}